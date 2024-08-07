using System.ComponentModel.DataAnnotations;

namespace Lontra;

/// <summary>
/// Provides a method to separate data across independent groups of users via discriminator column.
/// Tenants ensure segregation of data in a non-sharded ("single") database.
/// Typically, a user in one Tenant should never be able to see data in other Tenants.
/// </summary>
/// <remarks>
/// Tenants are used extensively in SaaS applications (users cannot see data of other users) and in
/// multi-organization applications (users in one organization cannot see the data of other organizations).
/// <para>
/// See: <seealso href="https://learn.microsoft.com/en-us/ef/core/miscellaneous/multitenancy">Multi-Tenancy in EF Core</seealso>
/// </para>
/// </remarks>
public class Tenant : IEntity<TenantId>, ISoftDeleteEntity
{
    public TenantId Id { get; set; } = null!;

    [MaxLength(200)]
    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }
}
