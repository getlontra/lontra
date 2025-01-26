using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lontra.EFCore.Utils;

internal static class EntityTypeBuilderUtils
{
    internal static void ConfigureEntity<T, TId>(EntityTypeBuilder<T> builder)
        where T : class, IEntity<TId>
    {
        if (Attribute.IsDefined(typeof(T).GetProperty(nameof(IEntity<TId>.Id), typeof(TId))!, typeof(NotMappedAttribute)))
        {
            // Do not configure Id when marked with attribute [NotMapped]
            return;
        }

        builder.HasKey(e => e.Id);

        if (typeof(TId) == typeof(long) || typeof(TId) == typeof(int))
        {
            // Auto-increment numeric IDs
            builder
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
        }
    }

    internal static void ConfigureEntityIdentifier<T, TId, TIdValue>(EntityTypeBuilder<T> builder)
        where T : class, IEntity<TId>
        where TId : Identifier<TIdValue>
    {
        if (Attribute.IsDefined(typeof(T).GetProperty(nameof(IEntity<TId>.Id), typeof(TId))!, typeof(NotMappedAttribute)))
        {
            // Do not configure Id when marked with attribute [NotMapped]
            return;
        }

        if (typeof(TIdValue) == typeof(long) || typeof(TIdValue) == typeof(int))
        {
            // Auto-increment numeric IDs
            builder
                .Property(e => e.Id)
                .HasConversion(id => id.Value, v => (TId)typeof(TId).GetConstructor(new[] { typeof(TIdValue) })!.Invoke(new[] { (object?)v }))
                .ValueGeneratedOnAdd();
        }
        else
        {
            builder
                .Property(e => e.Id)
                .HasConversion(id => id.Value, v => (TId)typeof(TId).GetConstructor(new[] { typeof(TIdValue) })!.Invoke(new[] { (object?)v }));
        }
    }

    internal static void ConfigureAuditableEntity<T>(EntityTypeBuilder<T> builder)
        where T : class, IAuditableEntity
    {
        builder
            .HasOne(e => e.CreatedBy)
            .WithMany()
            .HasForeignKey(e => e.CreatedBy)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(e => e.UpdatedBy)
            .WithMany()
            .HasForeignKey(e => e.UpdatedBy)
            .OnDelete(DeleteBehavior.NoAction);
    }

    internal static void ConfigureMultiTenantEntity<T>(EntityTypeBuilder<T> builder)
        where T : class, IMultiTenantEntity
    {
        builder
            .Property(e => e.TenantId)
            .HasColumnOrder(0);

        builder
            .HasOne(e => e.Tenant)
            .WithMany()
            .HasForeignKey(u => u.TenantId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(true);

        builder
            .Navigation(e => e.Tenant)
            .AutoInclude(true)
            .IsRequired(true);
    }

    internal static void ConfigureSoftDeleteEntity<T>(EntityTypeBuilder<T> builder)
        where T : class, ISoftDeleteEntity
    {
        builder.Property(e => e.IsActive);
    }

    internal static void ConfigureQueryFilter_MT_SD<T>(ILontraAppDbContext dbContext, EntityTypeBuilder<T> builder)
        where T : class, IMultiTenantEntity, ISoftDeleteEntity
    {
        builder.HasQueryFilter(e => e.IsActive && e.TenantId == dbContext.UserContext.CurrentTenantId);
    }

    internal static void ConfigureQueryFilter_MT<T>(ILontraAppDbContext dbContext, EntityTypeBuilder<T> builder)
        where T : class, IMultiTenantEntity
    {
        builder.HasQueryFilter(e => e.TenantId == dbContext.UserContext.CurrentTenantId);
    }

    internal static void ConfigureQueryFilter_SD<T>(ILontraAppDbContext dbContext, EntityTypeBuilder<T> builder)
        where T : class, ISoftDeleteEntity
    {
        builder.HasQueryFilter(e => e.IsActive);
    }

}
