using MicroBlog.Common.Validations.Posts;

namespace MicroBlog.Common;

public static class DependencyInjectionExtension
{
    public static void AddValidation(this IServiceCollection services)
    {
        services.AddScoped<IValidator<PostEditDto>, PostEditDtoValidator>();
        services.AddScoped<IValidator<YouTubeRecommendationDto>, YouTubeRecommendationDtoValidator>();
    }
}