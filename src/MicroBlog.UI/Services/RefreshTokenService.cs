namespace MicroBlog.UI.Services;

public class RefreshTokenService
{
    private readonly AuthenticationStateProvider _authProvider;
    private readonly IAuthService _authService;
    private readonly ILocalStorageService _localStorage;

    public RefreshTokenService(AuthenticationStateProvider authProvider, IAuthService authService, ILocalStorageService localStorage)
    {
        _authProvider = authProvider;
        _authService = authService;
        _localStorage = localStorage;
    }

    public async Task<string> TryRefreshToken()
    {
        // проверка, истекает ли токен
        var authState = await _authProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        var exp = user.FindFirst(c => c.Type.Equals("exp"))?.Value;
        var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
        var timeUtc = DateTime.UtcNow;

        var diff = expTime - timeUtc;
        if (diff.TotalMinutes <= 2)
        {
            return await _authService.RefreshToken();
        }

        //если нет, то возвращаем токен
        var token = await _localStorage.GetItemAsync<AuthResponseDto>("token");
        return token?.Token ?? string.Empty;
    }
}
