namespace Lontra.Core.Implementation;

/// <summary>
/// Default implementation of the <see cref="IUserContext"/> for scenarios where user information
/// can be stored in-memory alongside the running thread.
/// </summary>
public class InMemoryUserContext : IUserContext
{
    public TenantId? CurrentTenantId { get; set; }

    public UserId? CurrentUserId { get; set; }

    public string? CurrentUserName { get; set; }

    public string? CurrentUserGivenName { get; set; }

    public string? CurrentUserEmail { get; set; }

    public Task SetCurrentUser(UserContextData user, bool keepCurrentTenant, CancellationToken _)
    {
        if (!keepCurrentTenant && user.TenantId == null)
        {
            throw new ArgumentNullException(nameof(user.TenantId), "");
        }

        CurrentUserId = user.UserId;
        CurrentUserName = user.UserName;
        CurrentUserGivenName = user.UserGivenName;
        CurrentUserEmail = user.UserEmail;

        if (!keepCurrentTenant)
        {
            CurrentTenantId = user.TenantId;
        }

        return Task.CompletedTask;
    }

    public Task ClearCurrentUser(CancellationToken _)
    {
        CurrentTenantId = null;

        CurrentUserId = null;
        CurrentUserName = null;
        CurrentUserGivenName = null;
        CurrentUserEmail = null;

        return Task.CompletedTask;
    }

    public Task SwitchTenant(TenantId tenantId, CancellationToken _)
    {
        CurrentTenantId = tenantId;
        return Task.CompletedTask;
    }
}
