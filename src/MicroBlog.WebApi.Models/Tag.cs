namespace MicroBlog.WebApi.Models;

public class Tag
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

    public ICollection<Post> Posts { get; set; } = new List<Post>();
}