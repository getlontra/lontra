using Microsoft.EntityFrameworkCore;

namespace Lontra.EFCore;

public interface ILontraAppDbContext
{
    public IUserContext UserContext { get; }


    public bool HasSaveChangesFailure { get; set; }


    public DbSet<Tenant> Tenant { get; set; }

    public DbSet<User> User { get; set; }
}
