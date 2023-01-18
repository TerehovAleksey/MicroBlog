namespace MicroBlog.Common;

public static class DependencyInjectionExtension
{
    public static void AddValidation(this IServiceCollection services)
    {
        //services.AddScoped<IValidator<CurrencyDto>, CurrencyDtoValidator>();
    }
}