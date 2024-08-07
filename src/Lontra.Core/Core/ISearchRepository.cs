using System.Linq.Expressions;

namespace Lontra;

public interface ISearchRepository<T, F, S>
{
    public Task<IReadOnlyCollection<DT>> SearchAsync<DT>(Expression<Func<T, DT>> selector, F filter, Pagination pagination, S sort, CancellationToken cancellationToken = default);
}
