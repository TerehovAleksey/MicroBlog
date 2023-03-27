namespace MicroBlog.UI.Helpers;

public static class HttpClientInterceptorHelper
{
    public static void Subscribe(WebAssemblyHost host)
    {
        var httpInterceptor = host.Services.GetService<HttpInterceptorService>();
        httpInterceptor?.RegisterEvent();
    }
}