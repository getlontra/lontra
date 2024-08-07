namespace Lontra;

public interface ICreateWithActionRepository<T, TId>
{
    public Task<TId> CreateWithActionAsync(Action<T> createAction, CancellationToken cancellationToken = default);
}
