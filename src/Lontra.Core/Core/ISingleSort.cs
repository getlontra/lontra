namespace Lontra;

public interface ISingleSort : ISort
{
    public SortDirection? Direction { get; set; }

    public string? Key { get; set; }
}
