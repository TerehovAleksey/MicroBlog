namespace MicroBlog.WebApi.Core.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users")
            .HasKey(x => x.Id);

        builder.Property(x => x.UserName)
            .HasMaxLength(50);

        builder.Property(x => x.NormalizedUserName)
            .HasMaxLength(50);

        builder.Property(x => x.Email)
            .HasMaxLength(50);

        builder.Property(x => x.NormalizedEmail)
            .HasMaxLength(50);

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(50);

        builder.Property(x => x.RefreshToken)
            .HasMaxLength(1050);
    }
}