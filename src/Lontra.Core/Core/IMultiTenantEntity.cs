namespace Lontra;

public interface IMultiTenantEntity
{
    public TenantId TenantId { get; set; }

    public Tenant Tenant { get; set; }
}
