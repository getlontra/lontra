namespace Lontra.Core.Implementation;

/// <summary>
/// Default implementation of the <see cref="IUserContext"/> for scenarios where user information
/// can be stored in-memory alongside the running thread.
/// </summary>
public class InMemoryUserContext : IUserContext
{
    public TenantId? CurrentTenantId { get; set; }

    public UserId? CurrentUserId { get; set; }

    public string? CurrentUsername { get; set; }

    public string? CurrentUserGivenName { get; set; }

    public string? CurrentUserEmail { get; set; }

    public Task SetCurrentUser(User user, bool keepCurrentTenant, CancellationToken _)
    {
        CurrentUserId = user.Id;
        CurrentUsername = user.Username;
        CurrentUserGivenName = user.GivenName;
        CurrentUserEmail = user.Email;

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
        CurrentUsername = null;
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
