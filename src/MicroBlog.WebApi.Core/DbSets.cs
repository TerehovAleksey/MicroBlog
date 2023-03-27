namespace MicroBlog.WebApi.Core;

public sealed partial class AppDbContext
{
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<YoutubeRecommendation> YoutubeRecommendations => Set<YoutubeRecommendation>();
}