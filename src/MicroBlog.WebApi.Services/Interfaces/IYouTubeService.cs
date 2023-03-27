namespace MicroBlog.WebApi.Services.Interfaces;

public interface IYouTubeService
{
    public Task<IEnumerable<string>> GetRecommendationsAsync(int count = 2);
    public Task<IEnumerable<YouTubeDetailedRecommendationDto>> GetAllRecommendationsAsync();
    public Task AddRecommendationAsync(YouTubeRecommendationDto dto);
    public Task<bool> UpdateRecommendationAsync(Guid id, YouTubeRecommendationDto dto);
    public Task<bool> DeleteRecommendationAsync(Guid id);
}