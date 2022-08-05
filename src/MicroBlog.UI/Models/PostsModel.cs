namespace MicroBlog.UI.Models;

public record PostsModel
{
    public List<PostCardModel> Posts { get; init; } = new();
    public int Page { get; init; }
    public int TotalPages { get; init; }
}