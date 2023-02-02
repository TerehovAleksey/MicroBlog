namespace MicroBlog.Common.Dtos;

/// <summary>
/// Базовая информация для отображения поста в списках
/// </summary>
public record PostListItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Author { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string Cover { get; set; } = default!;
    public string MainTag { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime DatePublished { get; set; }
}

public record PostNavigationItem
{
    public string Title { get; set; } = default!;
    public string Slug { get; set; } = default!;
}

public record PostNavigationDto
{
    public PostNavigationItem? PreviousPost { get; set; }
    public PostNavigationItem? NextPost { get; set; }
}

/// <summary>
/// Основное содержимое поста (публичная модель)
/// </summary>
public record PostDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Author { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string Cover { get; set; } = default!;
    public DateTime DatePublished { get; set; }
    public string Description { get; set; } = default!;
    public string Content { get; set; } = default!;
    public IEnumerable<string> Tags { get; set; } = new List<string>();
}

public record PostEditListItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public bool IsFeatured { get; set; }
    public DateTime? DatePublished { get; set; }
}

public record PostEditDto
{
    public string Title { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string? Cover { get; set; }
    public string Description { get; set; } = default!;
    public string Content { get; set; } = default!;
    public IEnumerable<string> Tags { get; set; } = new List<string>();
}