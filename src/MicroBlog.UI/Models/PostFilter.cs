namespace MicroBlog.UI.Models;

public class PostFilter
{
    public int Page { get; set; }
    public string? Tag { get; set; }
    public string? Author { get; set; }
    public string? Date { get; set; }
    public string? Query { get; set; }
}