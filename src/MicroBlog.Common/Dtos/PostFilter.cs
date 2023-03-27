namespace MicroBlog.Common.Dtos;

public class PostFilter
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? Tag { get; set; }
    public string? Author { get; set; }
    public DateTime? Date { get; set; }
    public string? Query { get; set; }

    public PostFilter()
    {
        Page = 1;
        PageSize = 10;
    }
}