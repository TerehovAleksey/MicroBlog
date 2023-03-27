namespace MicroBlog.UI.Core.UiModels;

public record SelectablePostModel
{
    public Guid Id { get; set; }
    public DateTime? DatePublished { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsSelected { get; set; }
    public string Title { get; set; } = default!;
}