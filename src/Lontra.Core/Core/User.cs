using System.ComponentModel.DataAnnotations;

namespace Lontra;

/// <summary>
/// An end-user of the application.
/// </summary>
public class User : IEntity<UserId>, IMultiTenantEntity, ISoftDeleteEntity
{
    public TenantId TenantId { get; set; } = null!;
    public Tenant Tenant { get; set; } = null!;

    public UserId Id { get; set; } = null!;

    [MaxLength(200)]
    public string Username { get; set; } = null!;

    [MaxLength(200)]
    public string GivenName { get; set; } = null!;

    [MaxLength(200)]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [MaxLength(200)]
    public string ExternalId { get; set; } = null!;

    public bool IsActive { get; set; }
}
