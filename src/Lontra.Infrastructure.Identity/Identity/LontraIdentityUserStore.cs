using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lontra.Identity;

public class LontraIdentityUserStore : UserStore<LontraIdentityUser, IdentityRole<long>, LontraIdentityDbContext, long>
{
    public LontraIdentityUserStore(LontraIdentityDbContext context, IdentityErrorDescriber? describer = null) : base(context, describer) { }

    public override async Task<LontraIdentityUser?> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var id = ConvertIdFromString(userId);
        return await Context
            .Users
            .IgnoreQueryFilters()
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync();
    }

    public override async Task<LontraIdentityUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        return await Context
            .Users
            .IgnoreQueryFilters()
            .Where(u => u.NormalizedUserName == normalizedUserName)
            .FirstOrDefaultAsync();
    }
}
