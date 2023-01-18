namespace MicroBlog.UI.Core.Services;

public class FakeYoutubeService : IYoutubeService
{
    public async Task<string> GetLastVideo()
    {
        await Task.Delay(100);
        return "https://www.youtube.com/embed/WToQ51KV0_Q";
    }

    public async Task<string> GetLastRecommendation()
    {
        await Task.Delay(50);
        return "https://www.youtube.com/embed/M-cUZhCh_V0";
    }
}