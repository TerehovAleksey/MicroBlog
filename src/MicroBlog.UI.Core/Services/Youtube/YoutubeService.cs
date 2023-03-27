using MicroBlog.Common.Dtos.Youtube;

namespace MicroBlog.UI.Core.Services.Youtube;

public class YoutubeService : IYoutubeService
{
    private readonly HttpClient _httpClient;

    public YoutubeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<IEnumerable<string>?> GetLastVideosAsync() => 
        _httpClient.GetFromJsonAsync<IEnumerable<string>>("youtube/my");

    public Task<IEnumerable<YouTubeDetailedRecommendationDto>?> GetAllRecommendationsAsync() =>
        _httpClient.GetFromJsonAsync<IEnumerable<YouTubeDetailedRecommendationDto>>("youtube");

    public Task<IEnumerable<string>?> GetLastRecommendationsAsync() =>
        _httpClient.GetFromJsonAsync<IEnumerable<string>>("youtube/random");

    public Task CreateRecommendationAsync(YouTubeRecommendationDto dto) =>
        _httpClient.PostAsJsonAsync("youtube", dto);
    
    
}