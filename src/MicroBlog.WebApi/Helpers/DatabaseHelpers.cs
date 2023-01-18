namespace MicroBlog.WebApi.Helpers;

public static class DatabaseHelpers
{
    public static async Task DatabaseMigrate(this IHost app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();
    }
}
