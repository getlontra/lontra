namespace Lontra;

public interface IUpdateWithActionRepository<T, TId>
{
    public Task UpdateWithActionAsync(TId id, Action<T> updateAction, CancellationToken cancellationToken = default);
}
