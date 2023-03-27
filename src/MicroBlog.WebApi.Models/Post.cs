namespace MicroBlog.WebApi.Models;

public class Post
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public PostType PostType { get; set; }
    public bool IsFeatured { get; set; }

    public string Title { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Content { get; set; } = default!;
    public string? Cover { get; set; }
    
    public int PostViews { get; set; }
    public double Rating { get; set; }
    
    public DateTime? DatePublished { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }

    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}