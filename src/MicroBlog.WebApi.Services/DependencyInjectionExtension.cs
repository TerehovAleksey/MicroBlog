namespace MicroBlog.WebApi.Services;

public static class DependencyInjectionExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IPostService, PostService>();
        services.AddTransient<IYouTubeService, YouTubeService>();
    }
}