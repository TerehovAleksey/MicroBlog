using MicroBlog.Common.Dtos.Youtube;

namespace MicroBlog.UI.Core.Services.Youtube;

public interface IYoutubeService
{
    public Task<IEnumerable<string>?> GetLastVideosAsync();
    public Task<IEnumerable<YouTubeDetailedRecommendationDto>?> GetAllRecommendationsAsync();
    public Task<IEnumerable<string>?> GetLastRecommendationsAsync();
    public Task CreateRecommendationAsync(YouTubeRecommendationDto dto);
}