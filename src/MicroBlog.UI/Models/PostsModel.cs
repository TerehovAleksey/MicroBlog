namespace MicroBlog.UI.Models;

public record PostsModel
{
    public IEnumerable<PostCardModel> Posts { get; init; } = default!;
    public int Page { get; init; }
    public int TotalPages { get; init; }
}