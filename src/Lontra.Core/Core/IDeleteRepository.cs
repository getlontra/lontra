namespace Lontra;

public interface IDeleteRepository<TId>
{
    public Task DeleteAsync(TId id, CancellationToken cancellationToken = default);
}
