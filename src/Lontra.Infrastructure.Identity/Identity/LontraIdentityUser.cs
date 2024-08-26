using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Lontra.Identity;

public class LontraIdentityUser : IdentityUser<UserId>, IUser
{
    public TenantId TenantId { get; set; } = null!;
    public Tenant Tenant { get; set; } = null!;


    // ASP.NET Core Identity fields

    [MaxLength(200)]
    [ProtectedPersonalData]
    public override string? UserName { get; set; }

    [MaxLength(200)]
    [ProtectedPersonalData]
    public override string? Email { get; set; }


    // Other fields

    [MaxLength(200)]
    [ProtectedPersonalData]
    public string? GivenName { get; set; }

    [MaxLength(200)]
    [ProtectedPersonalData]
    public string? ExternalId { get; set; }

    public bool IsActive { get; set; }
}
