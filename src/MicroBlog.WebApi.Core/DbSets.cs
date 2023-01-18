namespace MicroBlog.WebApi.Core;

public sealed partial class AppDbContext
{
    public DbSet<Tag> Tags => Set<Tag>();
}