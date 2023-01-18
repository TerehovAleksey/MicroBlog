namespace MicroBlog.UI.Models;

public class BasePostModel
{
    public string Title { get; init; } = default!;
    public string Cover { get; init; } = default!;
    public DateTime Date { get; init; }
    public string Slug { get; set; } = default!;
}