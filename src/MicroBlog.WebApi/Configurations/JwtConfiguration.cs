namespace MicroBlog.WebApi.Configurations;

public static class JwtConfiguration
{
    public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection("JwtTokenConfig");
        services.Configure<JwtTokenConfig>(jwtSection);
        var jwtConfig = jwtSection.Get<JwtTokenConfig>();

        if (jwtConfig is null)
        {
            throw new Exception("JWT Configuration information wasn't found");
        }

        var key = Encoding.UTF8.GetBytes(jwtConfig.SecretKey);

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtConfig.Issuer,
                ValidAudience = jwtConfig.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });
    }
}
