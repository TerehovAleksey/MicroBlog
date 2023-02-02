namespace MicroBlog.WebApi.Core.Configurations;

internal class YoutubeRecommendationConfiguration  : IEntityTypeConfiguration<YoutubeRecommendation>
{
    public void Configure(EntityTypeBuilder<YoutubeRecommendation> builder)
    {
        builder.ToTable("YoutubeRecommendations")
            .HasKey(x => x.Id);
        
        builder.Property(x => x.VideoId)
            .IsRequired()
            .HasMaxLength(160);
        
        builder.Property(x => x.VideoName)
            .HasMaxLength(250);
        
        builder.Property(x => x.ChannelName)
            .HasMaxLength(50);
        
        builder.Property(x => x.ChannelLink)
            .HasMaxLength(160);

        builder.Property(x => x.IsEnable)
            .IsRequired();
    }
}