namespace MicroBlog.UI.Core.Services;

public interface IYoutubeService
{
    public Task<string> GetLastVideo();
    public Task<string> GetLastRecommendation();
}