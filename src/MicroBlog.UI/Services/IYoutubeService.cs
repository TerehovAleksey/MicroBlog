namespace MicroBlog.UI.Services;

public interface IYoutubeService
{
    public Task<string> GetLastVideo();
    public Task<string> GetLastRecommendation();
}