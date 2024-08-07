namespace Lontra;

public class Pagination
{
    private int _pageNumber = 1;
    private int _pageSize = 10;

    private long _totalRecords = 0;

    public virtual int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = Math.Max(value, 1);
    }

    public virtual int PageSize
    {
        get => _pageSize;
        set => _pageSize = value == -1 ? -1 : Math.Min(Math.Max(1, value), 1000);
    }

    public virtual long TotalRecords
    {
        get => _totalRecords;
        set => _totalRecords = Math.Max(value, 0);
    }
}
