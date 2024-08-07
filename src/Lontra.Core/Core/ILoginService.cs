namespace Lontra;

public interface ILoginService
{
    /// <summary>
    /// Attempt to login as user with ID <paramref name="userId"/>.
    /// This action should initialize the <see cref="IUserContext"/> values.
    /// </summary>
    /// <param name="userId">User to attempt to login as.</param>
    public Task TryLoginAsync(UserId userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Attempt to login as user with the given username/password.
    /// This action should initialize the <see cref="IUserContext"/> values.
    /// </summary>
    /// <param name="userId">User to attempt to login as.</param>
    public Task<UserId> TryLoginAsync(string username, string password, CancellationToken cancellationToken = default);

    /// <summary>
    /// Logout the current user, according to the active <see cref="IUserContext"/>.
    /// </summary>
    public Task LogoutAsync(CancellationToken cancellationToken = default);
}
