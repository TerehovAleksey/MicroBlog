namespace MicroBlog.WebApi.Core;

public sealed partial class AppDbContext : IdentityDbContext<User, Role, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        #region Identity
        
        modelBuilder.ApplyConfiguration(new UserClaimConfiguration());
        modelBuilder.ApplyConfiguration(new UserLoginConfiguration());
        modelBuilder.ApplyConfiguration(new UserTokenConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        modelBuilder.ApplyConfiguration(new RoleClaimConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());

        #endregion Identity
        
        #region Configurations

         modelBuilder.ApplyConfiguration(new TagConfiguration());
         modelBuilder.ApplyConfiguration(new PostConfiguration());
         modelBuilder.ApplyConfiguration(new YoutubeRecommendationConfiguration());

         #endregion Configurations
    }
}