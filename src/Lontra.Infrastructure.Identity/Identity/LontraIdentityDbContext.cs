using Lontra.EFCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lontra.Identity;

/// <summary>
/// Default implementation of a <see cref="DbContext"/> implementing both the Lontra DbContext spec and
/// serving as an ASP.NET Identity Store.
/// </summary>
/// <remarks>
/// The scaffolded <see cref="DbContext"/> will contain both the ASP.NET Identity tables (AspNetUsers, AspNetRoles...)
/// and the Lontra Tenant/User tables.
/// </remarks>
public class LontraIdentityDbContext : IdentityDbContext<LontraIdentityUser, IdentityRole<long>, long>, ILontraAppDbContext
{
    public IUserContext UserContext { get; }

    public DbSet<Tenant> Tenant { get; set; } = null!;

    public bool HasSaveChangesFailure { get; set; }

    /// <summary>
    /// Initializes a new <see cref="DbContext"/> serving as an ASP.NET Identity Store with default options.
    /// </summary>
    /// <param name="userContext">UserContext implementation to be injected.</param>
    public LontraIdentityDbContext(IUserContext userContext) : base()
    {
        UserContext = userContext;
    }

    /// <summary>
    /// Initializes a new <see cref="DbContext"/> serving as an ASP.NET Identity Store,
    /// with user-provided options.
    /// </summary>
    /// <param name="userContext">UserContext implementation to be injected.</param>
    /// <param name="options"><inheritdoc cref="IdentityDbContext(DbContextOptions)"/></param>
    public LontraIdentityDbContext(IUserContext userContext, DbContextOptions<LontraIdentityDbContext> options) : base(options)
    {
        UserContext = userContext;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        LontraDbContextExtensions.FinalizeOnConfiguring(this, optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        LontraDbContextExtensions.FinalizeOnModelCreating(this, modelBuilder);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        LontraDbContextExtensions.PrepareSaveChanges(this);

        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        LontraDbContextExtensions.PrepareSaveChanges(this);

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
