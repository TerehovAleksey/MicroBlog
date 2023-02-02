namespace MicroBlog.WebApi.Core.Configurations;

internal class PostConfiguration  : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts")
            .HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();
        
        builder.Property(x => x.PostType)
            .IsRequired();
        
        builder.Property(x => x.IsFeatured)
            .IsRequired();

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(160);
        
        builder.Property(x => x.Slug)
            .IsRequired()
            .HasMaxLength(160);
        
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(450);
        
        builder.Property(x => x.Content)
            .IsRequired();
        
        builder.Property(x => x.Cover)
            .HasMaxLength(160);
        
        builder.Property(x => x.PostViews)
            .IsRequired();
        
        builder.Property(x => x.Rating)
            .IsRequired();
    }
}