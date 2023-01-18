namespace WebApi.Paging;

public class PagedList<T> : List<T> where T : class
{
    public MetaData MetaData { get; }
    
    public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        MetaData = new MetaData(count, pageSize, pageNumber);
        AddRange(items);
    }

    public PagedList(IEnumerable<T> items, MetaData metaData) : this(items)
    {
        MetaData = metaData;
    }
    
    public PagedList(IEnumerable<T> items) : this()
    {
        AddRange(items);
    }

    public PagedList()
    {
        MetaData = new();
    }
}