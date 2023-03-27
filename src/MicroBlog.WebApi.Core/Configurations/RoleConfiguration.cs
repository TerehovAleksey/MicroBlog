namespace MicroBlog.WebApi.Core.Configurations;

internal class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles")
            .HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(50);

        builder.Property(x => x.NormalizedName)
            .HasMaxLength(50);
    }
}