using Lontra.Core;

namespace Lontra;

/// <summary>
/// Provides information on the current logged-in user.
/// The UserContext should be initialized on login and cleared on logout.
/// </summary>
public interface IUserContext
{
    /// <summary>
    /// The <see cref="TenantId"/> for the current user, if logged in.
    /// </summary>
    /// <remarks>
    /// In Multi-Tenant applications, this will affect what data is visible to the user.
    /// If there is no logged user, this will be null.
    /// </remarks>
    public TenantId? CurrentTenantId { get; }

    /// <summary>
    /// The <see cref="UserId"/> of the current user, if logged in.
    /// </summary>
    /// <remarks>
    /// If there is no logged user, this will be null.
    /// </remarks>
    public UserId? CurrentUserId { get; }

    /// <summary>
    /// Username of the current user, as used for login.
    /// </summary>
    /// <remarks>
    /// In some applications, this may be the same as the <see cref="CurrentUserEmail" />.
    /// If there is no logged user, this will be null.
    /// <para>
    /// <example>Example: domain\john.doe</example>
    /// </para>
    /// </remarks>
    public string? CurrentUserName { get; }

    /// <summary>
    /// The Given Name of the current user, if logged in.
    /// </summary>
    /// <remarks>
    /// If there is no logged user, this will be null.
    /// <para>
    /// <example>Example: John Doe</example>
    /// </para>
    /// </remarks>
    public string? CurrentUserGivenName { get; }

    /// <summary>
    /// The e-mail of the current user.
    /// </summary>
    /// <remarks>
    /// If there is no logged user, this will be null.
    /// <para>
    /// <example>Example: johndoe@example.com</example>
    /// </para>
    /// </remarks>
    public string? CurrentUserEmail { get; }

    /// <summary>
    /// Sets the User for the currently executing thread/request.
    /// </summary>
    /// <param name="user">User information to set.</param>
    /// <param name="keepCurrentTenant">If true, do not switch to the user's Tenant.</param>
    /// <param name="cancellationToken">Cancellation Token to be observed.</param>
    public Task SetCurrentUser(UserContextData user, bool keepCurrentTenant = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Clears the User for the currently executing thread/request.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token to be observed.</param>
    public Task ClearCurrentUser(CancellationToken cancellationToken = default);

    /// <summary>
    /// Changes the <see cref="TenantId"/> for the current user in a Multi-Tenant application.
    /// Super users, such as Administrators, may have permissions that allow them to see information in different tenants.
    /// </summary>
    /// <param name="tenantId">Tenant ID to set for the current .</param>
    /// <param name="cancellationToken">Cancellation Token to be observed.</param>
    public Task SwitchTenant(TenantId tenantId, CancellationToken cancellationToken = default);
}
