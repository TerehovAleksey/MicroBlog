namespace MicroBlog.UI.Services;

public class HttpInterceptorService : IDisposable
{
    private readonly HttpClientInterceptor _interceptor;
    private readonly RefreshTokenService _refreshTokenService;
    private readonly NavigationManager _navManager;
    private readonly IAuthService _authService;
    public HttpInterceptorService(HttpClientInterceptor interceptor, RefreshTokenService refreshTokenService, NavigationManager navManager, IAuthService authService)
    {
        _interceptor = interceptor;
        _refreshTokenService = refreshTokenService;
        _navManager = navManager;
        _authService = authService;
    }
    public void RegisterEvent()
    {
        _interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;
        _interceptor.AfterSend += InterceptResponse;
    }

    private async Task InterceptBeforeHttpAsync(object _, HttpClientInterceptorEventArgs e)
    {
        var absPath = e.Request?.RequestUri?.AbsolutePath; 
        if (absPath != null && absPath.Contains("admin"))
        {
            var token = await _refreshTokenService.TryRefreshToken();
            if (!string.IsNullOrEmpty(token) && e.Request is not null)
            {
                e.Request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
            }
        }
    }
    private void InterceptResponse(object? _, HttpClientInterceptorEventArgs e)
    {
        var absPath = e.Request?.RequestUri?.AbsolutePath;
        if (!e.Response.IsSuccessStatusCode && absPath is not null && absPath.Contains("admin"))
        {
            if (e.Response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _authService.Logout(false);
                _navManager.NavigateTo("/401");
            }
        }
    }

    private void DisposeEvent()
    {
        _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
        _interceptor.AfterSend -= InterceptResponse;
    }

    public void Dispose()
    {
        DisposeEvent();
    }
}
