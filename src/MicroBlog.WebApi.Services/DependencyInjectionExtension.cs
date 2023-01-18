namespace MicroBlog.WebApi.Services;

public static class DependencyInjectionExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DomainToDtoMappingProfile), typeof(DtoToDomainMappingProfile));
 
        //services.AddTransient<IPostService, PostService>();
        services.AddTransient<IPostService, FakePostService>();
    }
}