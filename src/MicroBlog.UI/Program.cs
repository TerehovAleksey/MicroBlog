//https://micro-templatesyard.blogspot.com/

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(Config.SERVER_API_URI) }
    .EnableIntercept(sp));

builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddScoped<RefreshTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<HttpInterceptorService>();
builder.Services.AddHttpClientInterceptor();

builder.Services.AddMarkdownEditor();

builder.Services.AddSingleton<IFilterService, FilterService>();
builder.Services.AddSingleton<INavigationService, NavigationService>();
builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddTransient<IYoutubeService, YoutubeService>();
builder.Services.AddTransient<IFileService, FileService>();

var host = builder.Build();
HttpClientInterceptorHelper.Subscribe(host);
await host.RunAsync();