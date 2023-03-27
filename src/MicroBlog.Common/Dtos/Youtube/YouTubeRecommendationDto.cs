namespace MicroBlog.Common.Dtos.Youtube;

public record YouTubeRecommendationDto
{
    public string VideoId { get; set; } = default!;
    public string? VideoName { get; set; }
    public string? ChannelName { get; set; }
    public string? ChannelLink { get; set; }
}

public record YouTubeDetailedRecommendationDto(Guid Id, string VideoId, bool IsEnabled, string? VideoName, string? ChannelName, string? ChannelLink);
