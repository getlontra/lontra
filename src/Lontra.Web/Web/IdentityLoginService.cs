using Lontra.Core;
using Lontra.EFCore;
using Lontra.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;
using System.Security.Claims;

namespace Lontra.Web;

/// <summary>
/// Provides an implementation for <see cref="ILoginService"/> that delegates to AspNetCore.Identity services.
/// </summary>
/// <typeparam name="TDbContext">IdentityDbContext implementation</typeparam>
public class IdentityLoginService<TDbContext> : ILoginService
    where TDbContext : IdentityDbContext<LontraIdentityUser, IdentityRole<long>, long>, ILontraAppDbContext
{
    private readonly IUserContext _userContext;

    private readonly TDbContext _identityDbContext;

    private readonly UserManager<LontraIdentityUser> _userManager;
    private readonly SignInManager<LontraIdentityUser> _signInManager;

    public IdentityLoginService(
        IUserContext userContext,
        TDbContext identityDbContext,
        UserManager<LontraIdentityUser> userManager,
        SignInManager<LontraIdentityUser> signInManager
    )
    {
        _userContext = userContext;
        _identityDbContext = identityDbContext;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task TryLoginAsync(UserId userId, CancellationToken cancellationToken)
    {
        var user = await _identityDbContext
            .Users
            .Include(u => u.Tenant)
            .Where(u => new UserId(u.Id) == userId)
            .Where(u => u.IsActive)
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(cancellationToken) ?? throw UserIdNotFoundException(userId);

        EnsureUserValid(user, user.Tenant);

        await _signInManager.SignInAsync(user, new AuthenticationProperties());

        var userCtxData = new UserContextData()
        {
            TenantId = user.Tenant.Id,
            UserId = userId,
            UserName = user.UserName,
            UserGivenName = user.GivenName,
            UserEmail = user.Email,
        };

        await _userContext.SetCurrentUser(userCtxData, keepCurrentTenant: false, cancellationToken);
    }

    public async Task<UserId> TryLoginAsync(string username, string password, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(username) ?? throw UsernameNotFoundException(username);

        EnsureUserValid(user, user.Tenant);

        if (user.LockoutEnabled)
        {
            // Extend lockout

            DateTime lockoutEnd = DateTime.Now.Add(_userManager.Options.Lockout.DefaultLockoutTimeSpan);
            await _userManager.SetLockoutEndDateAsync(user, lockoutEnd);
            throw UserLockedOutException();
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, true);
        if (!signInResult.Succeeded)
        {
            throw InvalidPasswordException();
        }

        await _signInManager.SignInWithClaimsAsync(user, new AuthenticationProperties(), Array.Empty<Claim>());

        var userId = new UserId(user.Id);
        var userCtxData = new UserContextData()
        {
            TenantId = user.Tenant.Id,
            UserId = userId,
            UserName = user.UserName,
            UserGivenName = user.GivenName,
            UserEmail = user.Email
        };

        await _userContext.SetCurrentUser(userCtxData, keepCurrentTenant: false, cancellationToken);

        return userId;
    }

    public async Task LogoutAsync(CancellationToken cancellationToken)
    {
        await _signInManager.SignOutAsync();
        await _userContext.ClearCurrentUser(cancellationToken);
    }

    private void EnsureUserValid(IUser user, Tenant tenant)
    {
        if (!user.IsActive || !tenant.IsActive)
        {
            throw UserLockedOutException();
        }
    }

    private static AuthenticationException UserIdNotFoundException(UserId userId)
        => new AuthenticationException($"User with ID '{userId}' not found.");

    private static AuthenticationException UsernameNotFoundException(string username)
        => new AuthenticationException($"User with username '{username}' not found.");

    private static AuthenticationException InvalidPasswordException()
        => new AuthenticationException($"Invalid password.");

    private static AuthenticationException UserLockedOutException()
        => new AuthenticationException($"User is locked out.");
}
