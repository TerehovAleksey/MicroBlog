var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(Config.SERVER_API_URI) });

// builder.Services.AddBootstrap();
// builder.Services.AddValidation();
// builder.Services.AddHttpServices();
//
// builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
// builder.Services.AddScoped<RefreshTokenService>();
// builder.Services.AddScoped<IAuthService, AuthService>();
// builder.Services.AddScoped<ErrorHandlerService>();
//
// builder.Services.AddLocalization();
// builder.Services.AddAuthorizationCore();
// builder.Services.AddBlazoredLocalStorage();
//
// builder.Services.AddScoped<HttpInterceptorService>();
// builder.Services.AddHttpClientInterceptor();


builder.Services.AddSingleton<IFilterService, FilterService>();
builder.Services.AddSingleton<INavigationService, NavigationService>();
builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddTransient<IYoutubeService, FakeYoutubeService>();

await builder.Build().RunAsync();

//https://micro-templatesyard.blogspot.com/