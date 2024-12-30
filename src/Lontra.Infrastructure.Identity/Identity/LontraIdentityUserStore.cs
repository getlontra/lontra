using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lontra.Identity;

public class LontraIdentityUserStore<TDbContext> : UserStore<LontraIdentityUser, IdentityRole<long>, TDbContext, long>
    where TDbContext : LontraIdentityDbContext
{
    public LontraIdentityUserStore(TDbContext context, IdentityErrorDescriber? describer = null) : base(context, describer) { }

    public override IQueryable<LontraIdentityUser> Users
    {
        // Ensure Tenant filter is ignored during TryLogin()
        get => base.Users.IgnoreQueryFilters();
    }
}
