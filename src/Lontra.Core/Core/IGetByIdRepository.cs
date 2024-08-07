using System.Linq.Expressions;

namespace Lontra;

public interface IGetByIdRepository<T, TId>
{
    public Task<DT> GetByIdAsync<DT>(TId id, Expression<Func<T, DT>> selector, CancellationToken cancellationToken = default);
}
