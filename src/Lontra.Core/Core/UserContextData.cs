namespace Lontra.Core;

/// <summary>
/// Wraps data to pass to an <see cref="IUserContext"/> implementation to initialize
/// user data for the duration of the request.
/// </summary>
public class UserContextData
{
    public TenantId? TenantId { get; set; }

    public required UserId? UserId { get; set; }

    public string? UserName { get; set; }

    public string? UserGivenName { get; set; }

    public string? UserEmail { get; set; }
}
