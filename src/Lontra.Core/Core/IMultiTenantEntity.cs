namespace Lontra;

/// <summary>
/// <para>
/// Specifies that an entity is multi-tenant, and should separate data access to
/// users/groups of the same tenant.
/// </para>
/// <para>
/// The<see cref="TenantId"/> field provides isolation in a multi-tenant solution. <br/>
/// Users in one tenant cannot see the data of other tenants. <br/>
/// In Lontra, multi-tenancy is implemented with a discriminator column.
/// </para>
/// </summary>
public interface IMultiTenantEntity
{
    /// <summary>
    /// The Tenant associated with this entity instance. 
    /// </summary>
    /// <remarks>
    /// The TenantId field provides isolation in a multi-tenant solution. <br/>
    /// Users in one tenant cannot see the data of other tenants. <br/>
    /// In Lontra, multi-tenancy is implemented with a discriminator column. <br/>
    /// </remarks>
    /// <seealso cref="Lontra.Tenant"/>
    public TenantId TenantId { get; set; }

    /// <inheritdoc cref="TenantId" />
    public Tenant Tenant { get; set; }
}
