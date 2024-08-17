using Microsoft.EntityFrameworkCore;

namespace Lontra.EFCore;

public static class IQueryableExtensions
{
    public static IQueryable<T> WithPagination<T>(this IQueryable<T> queryable, Pagination pagination)
    {
        if (pagination.PageSize == -1)
        {
            // Get all results
            return queryable;
        }

        return queryable
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize);
    }

    public static Task<int> ExecuteSoftDeleteAsync<T>(this IQueryable<T> queryable, CancellationToken cancellationToken = default) where T : ISoftDeleteEntity
    {
        return queryable.ExecuteUpdateAsync(e => e.SetProperty(e => e.IsActive, false), cancellationToken);
    }
}
