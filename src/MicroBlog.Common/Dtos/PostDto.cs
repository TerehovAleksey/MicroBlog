namespace MicroBlog.Common.Dtos;

public record PostBaseDto
{
    public string Title { get; set; } = default!;
    public string Author { get; set; } = default!;
    public string Slug { get; set; } = default!;   
    public string Cover { get; set; } = default!;
    public DateTime DatePublished { get; set; }
}

public record PostLinkDto : PostBaseDto
{
    public string MainTag { get; set; } = default!;
    public string Description { get; set; } = default!;
}

public record PostDto : PostBaseDto
{
    public string Content { get; set; } = default!;
    public IEnumerable<TagDto>? Tags { get; set; }
}