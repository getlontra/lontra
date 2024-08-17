using Microsoft.EntityFrameworkCore;

namespace Lontra.EFCore.Utils;

internal static class DbContextEventUtils
{
    public static void RegisterDbContextEventHandlers<TDbCtx>(this TDbCtx dbContext)
        where TDbCtx : DbContext, ILontraAppDbContext
    {
        dbContext.SaveChangesFailed += new EventHandler<SaveChangesFailedEventArgs>(OnSaveChangesFailed<TDbCtx>);
    }

    // TODO

    static void OnSaveChangesFailed<TDbCtx>(object? sender, SaveChangesFailedEventArgs evt)
        where TDbCtx : DbContext, ILontraAppDbContext
    {
        if (sender == null)
        {
            throw new ArgumentNullException(nameof(sender));
        }

        TDbCtx dbCtx = (TDbCtx)sender;

        TDbCtx senderDbCtx = (sender as TDbCtx)!;
        senderDbCtx.HasSaveChangesFailure = true;
    }
}
