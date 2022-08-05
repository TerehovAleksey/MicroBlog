using MicroBlog.UI;
using MicroBlog.UI.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<IFilterService, FilterService>();
builder.Services.AddTransient<IPostService, FakePostService>();
builder.Services.AddTransient<IYoutubeService, FakeYoutubeService>();

await builder.Build().RunAsync();

//https://micro-templatesyard.blogspot.com/
