using MicroBlog.Common.Dtos;

namespace MicroBlog.UI.Core.UiModels;

public record PostModel
{
    public string Title { get; set; } = default!;
    public string Author { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string Cover { get; set; } = default!;
    public DateTime DatePublished { get; set; }
    public string MainTag { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Content { get; set; } = default!;
    public IEnumerable<string> Tags { get; set; } = new List<string>();
    public PostNavigationModel? PreviousPost { get ; set; }
    public PostNavigationModel? NextPost { get ; set; }

    public static PostModel FromPostDto(PostDto dto)
    {
        return new PostModel
        {
            Title = dto.Title,
            Author = dto.Author,
            Slug = dto.Slug,
            Cover = dto.Cover,
            DatePublished = dto.DatePublished,
            MainTag = dto.Tags?.FirstOrDefault()?.Name ?? string.Empty,
            //Description
            Content = dto.Content,
            Tags = dto.Tags?.Select(x => x.Name) ?? new List<string>()
        };
    }
}

public record PostNavigationModel(string Title, string Slug);
