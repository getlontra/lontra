using Lontra.Core.Reflection;
using Lontra.EFCore.Configurations;
using Lontra.EFCore.Utils;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Lontra.EFCore;

public static class LontraDbContextExtensions
{
    public static void FinalizeOnConfiguring<TDbCtx>(this TDbCtx dbContext, DbContextOptionsBuilder optionsBuilder)
        where TDbCtx : DbContext, ILontraAppDbContext
    {
        DbContextEventUtils.RegisterDbContextEventHandlers(dbContext);
    }

    public static void FinalizeOnModelCreating<TDbCtx>(this TDbCtx dbContext, ModelBuilder modelBuilder)
        where TDbCtx : DbContext, ILontraAppDbContext
    {
        modelBuilder.ApplyConfiguration(new TenantConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());

        // Ignore Identifier types in Model
        var idTypes = modelBuilder
            .Model
            .GetEntityTypes()
            .Where(t => t.ClrType.IsAssignableTo(typeof(IIdentifier)))
            .ToList();

        foreach (var idType in idTypes)
        {
            modelBuilder.Ignore(idType.ClrType);
        }

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // Construct an entityTypeBuilder via ```ModelBuilder.Entity<T>()``` with reflection

            object entityTypeBuilder = typeof(ModelBuilder)
                    .GetMethods()
                    .Where(m => nameof(ModelBuilder.Entity).Equals(m.Name) && m.IsGenericMethod && m.GetParameters().Length == 0)
                    .First()
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(modelBuilder, [])!;

            bool isEntity      = entityType.ClrType.IsAssignableTo(typeof(IIEntity));
            bool isAuditable   = entityType.ClrType.IsAssignableTo(typeof(IAuditableEntity));
            bool isMultiTenant = entityType.ClrType.IsAssignableTo(typeof(IMultiTenantEntity));
            bool isSoftDelete  = entityType.ClrType.IsAssignableTo(typeof(ISoftDeleteEntity));

            if (isEntity)
            {
                var entityIdType = entityType.ClrType.GetProperty(nameof(IEntity<object>.Id))?.PropertyType!;

                typeof(EntityTypeBuilderUtils).GetMethod(nameof(EntityTypeBuilderUtils.ConfigureEntity), BindingFlags.Static | BindingFlags.NonPublic)!
                    .MakeGenericMethod(entityType.ClrType, entityIdType)
                    .Invoke(null, new[] { entityTypeBuilder });


                if (entityIdType.BaseType?.IsAssignableTo(typeof(IIdentifier)) ?? false)
                {
                    typeof(EntityTypeBuilderUtils).GetMethod(nameof(EntityTypeBuilderUtils.ConfigureEntityIdentifier), BindingFlags.Static | BindingFlags.NonPublic)!
                        .MakeGenericMethod(entityType.ClrType, entityIdType, entityIdType.BaseType!.GenericTypeArguments[0])
                        .Invoke(null, new[] { entityTypeBuilder });
                }
            }

            if (isAuditable)
            {
                typeof(EntityTypeBuilderUtils).GetMethod(nameof(EntityTypeBuilderUtils.ConfigureAuditableEntity), BindingFlags.Static | BindingFlags.NonPublic)!
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(null, new[] { entityTypeBuilder });
            }

            if (isMultiTenant)
            {
                typeof(EntityTypeBuilderUtils).GetMethod(nameof(EntityTypeBuilderUtils.ConfigureMultiTenantEntity), BindingFlags.Static | BindingFlags.NonPublic)!
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(null, new[] { entityTypeBuilder });
            }

            if (isSoftDelete)
            {
                typeof(EntityTypeBuilderUtils).GetMethod(nameof(EntityTypeBuilderUtils.ConfigureSoftDeleteEntity), BindingFlags.Static | BindingFlags.NonPublic)!
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(null, new[] { entityTypeBuilder });
            }

            if (isSoftDelete || isMultiTenant)
            {
                string queryFilterMethod;
                if (isSoftDelete)
                {
                    queryFilterMethod = isMultiTenant ? nameof(EntityTypeBuilderUtils.ConfigureQueryFilter_MT_SD) : nameof(EntityTypeBuilderUtils.ConfigureQueryFilter_SD);
                }
                else
                {
                    queryFilterMethod = nameof(EntityTypeBuilderUtils.ConfigureQueryFilter_MT);
                }

                typeof(EntityTypeBuilderUtils).GetMethod(queryFilterMethod, BindingFlags.Static | BindingFlags.NonPublic)!
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(null, new[] { dbContext, entityTypeBuilder });
            }
        }
    }

    public static void PrepareSaveChanges<TDbCtx>(this TDbCtx dbContext, UserId? userIdOverride = null, DateTime? timestampOverride = null)
        where TDbCtx : DbContext, ILontraAppDbContext
    {
        UserId userId = (userIdOverride != null) ? userIdOverride : dbContext.UserContext.CurrentUserId!;
        DateTime timestamp = (timestampOverride != null) ? timestampOverride.Value : DateTime.UtcNow;

        dbContext.ChangeTracker.DetectChanges();
        var added = dbContext.ChangeTracker
            .Entries()
            .Where(e => e.State == EntityState.Added && (e.Entity is IAuditableEntity || e.Entity is ISoftDeleteEntity))
            .Select(e => e.Entity)
            .ToList();

        foreach (var entity in added)
        {
            if (entity is IAuditableEntity auditableEntity)
            {
                auditableEntity.CreatedBy = userId;
                auditableEntity.CreatedOn = timestamp;
                auditableEntity.UpdatedBy = userId;
                auditableEntity.UpdatedOn = timestamp;
            }

            if (entity is ISoftDeleteEntity softDeleteEntity && !softDeleteEntity.IsActive)
            {
                softDeleteEntity.IsActive = true;
            }
        }

        var modified = dbContext.ChangeTracker
            .Entries()
            .Where(e => e.State == EntityState.Modified && (e.Entity is IAuditableEntity))
            .Select(e => e.Entity)
            .ToList();

        foreach (var entity in modified)
        {
            if (entity is IAuditableEntity auditableEntity)
            {
                auditableEntity.UpdatedBy = userId;
                auditableEntity.UpdatedOn = timestamp;
            }
        }
    }
}
