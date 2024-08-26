using System.ComponentModel.DataAnnotations;

namespace Lontra;

/// <summary>
/// An end-user of the application.
/// </summary>
public interface IUser : IEntity<UserId>, IMultiTenantEntity, ISoftDeleteEntity
{
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
    /// The human (given) name of the user.
    /// </summary>
    /// <remarks>
    /// Example: <c>John Doe</c>
    /// </remarks>
    [MaxLength(200)]
    public string? GivenName { get; set; }

    /// <summary>
    /// User e-mail contact.
    /// </summary>
    /// <remarks>
    /// Example: <c>john.doe@acme.com</c>
    /// </remarks>
    [MaxLength(200)]
    [EmailAddress]
    public string? Email { get; set; }

    [MaxLength(200)]
    public string? ExternalId { get; set; }
}
