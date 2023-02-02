namespace MicroBlog.WebApi.Models;

public class YoutubeRecommendation
{
    public Guid Id { get; set; }
    public string VideoId { get; set; } = default!;
    public string? VideoName { get; set; }
    public string? ChannelName { get; set; }
    public string? ChannelLink { get; set; }
    public bool IsEnable { get; set; }
}