namespace MicroBlog.WebApi;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        //AddIdentity должен стоять выше AddJwtBearer, иначе редирект при 401, 403
        //services.AddScoped<ITokenService, TokenService>();
        services.AddDatabaseContext(_configuration);
        services.AddServices();

        services.AddApiVersion();
        services.ConfigureIdentity();

        services.AddValidation();
        services.AddFluentValidationAutoValidation();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        //services.AddSwagger();
        //services.AddLocalization(options => options.ResourcesPath = "Resources");
        //services.AddJwt(_configuration);
        services.AddMemoryCache();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                policyBuilder =>
                {
                    policyBuilder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("X-Pagination");
                });
        });
    }

    public void Configure(IApplicationBuilder app)
    {
        if (_environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            //app.UseHttpsRedirection();
        }

        app.UseCors("AllowAll");

        app.UseSerilogRequestLogging();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
