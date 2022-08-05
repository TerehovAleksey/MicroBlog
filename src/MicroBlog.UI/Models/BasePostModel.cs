namespace MicroBlog.UI.Models;

public class BasePostModel
{
    public string Title { get; init; } = default!;
    public string ImageUri { get; init; } = default!;
    public DateOnly Date { get; init; }
}