using Lontra.Core.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Lontra.EFCore;

internal static class GenericRepository
{
    public static async Task<TId> CreateWithActionAsync<T, TId>(DbContext dbContext, Action<T> createAction, CancellationToken cancellationToken = default)
        where T : IEntity<TId>, new()
    {
        var entity = new T();

        dbContext.Add(entity);
        createAction(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public static async Task UpdateWithActionAsync<T, TId>(DbContext dbContext, TId id, Action<T> updateAction, Func<TId, Exception> notFoundFunc, CancellationToken cancellationToken = default)
        where T : class, IEntity<TId>
        where TId : IIdentifier
    {
        var entity = await dbContext
            .Set<T>()
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(cancellationToken) ?? throw notFoundFunc(id);

        dbContext.Attach(entity);
        updateAction(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public static async Task SoftDeleteAsync<T, TId>(DbContext dbContext, TId id, Func<TId, Exception> notFoundFunc, CancellationToken cancellationToken = default)
        where T : class, IEntity<TId>, ISoftDeleteEntity
        where TId : IIdentifier
    {
        var entity = await dbContext
            .Set<T>()
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(cancellationToken) ?? throw notFoundFunc(id);

        dbContext.Attach(entity);
        entity.IsActive = false;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
