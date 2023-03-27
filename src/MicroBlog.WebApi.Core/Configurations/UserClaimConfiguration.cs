namespace MicroBlog.WebApi.Core.Configurations;

internal class UserClaimConfiguration : IEntityTypeConfiguration<IdentityUserClaim<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> builder)
    {
        builder.ToTable("UserClaims")
            .HasKey(x => x.Id);
    }
}