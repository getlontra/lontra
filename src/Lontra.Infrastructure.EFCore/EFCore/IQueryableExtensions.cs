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

        if (pagination.PageNumber < 1)
        {
            // Ardalis: "If Skip is zero, avoid using Skip to generate more optimized SQL"
            //  ~ github.com/ardalis/Specification/blob/v8.0/Specification/src/Ardalis.Specification/Evaluators/PaginationEvaluator.cs#L15

            return queryable.Take(pagination.PageSize);
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
