using System.ComponentModel.DataAnnotations;

namespace Lontra.Core.Implementation;

/// <summary>
/// An end-user of the application.
/// </summary>
public class User : IUser
{
    public TenantId TenantId { get; set; } = null!;
    public Tenant Tenant { get; set; } = null!;

    public UserId Id { get; set; } = null!;

    /// <summary>
    /// The user identifier used for login. In some applications, this may
    /// be the same as the <see cref="Email"/> field.
    /// </summary>
    /// <remarks>
    /// Example: <c>acme\john.doe</c>
    /// </remarks>
    [MaxLength(200)]
    public string? UserName { get; set; }

    /// <summary>
    /// The given name (human name/friendly name) of the user.
    /// </summary>
    /// <remarks>
    /// Example: <c>John Doe</c>
    /// </remarks>
    [MaxLength(200)]
    public string? GivenName { get; set; } = null!;

    /// <summary>
    /// User e-mail contact.
    /// </summary>
    /// <remarks>
    /// Example: <c>john.doe@acme.com</c>
    /// </remarks>
    [MaxLength(200)]
    [EmailAddress]
    public string? Email { get; set; } = null!;

    [MaxLength(200)]
    public string? ExternalId { get; set; }

    public bool IsActive { get; set; }
}
