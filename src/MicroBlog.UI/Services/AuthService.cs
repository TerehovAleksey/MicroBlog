namespace MicroBlog.UI.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _client;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;
    private static readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    public AuthService(HttpClient client, ILocalStorageService localStorage,
        AuthenticationStateProvider authStateProvider)
    {
        _client = client;
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
    }

    public Task<ProfileDto?> GetProfileAsync() => _client.GetFromJsonAsync<ProfileDto>("auth/profile", _options);

    public async Task<bool> Login(UserForAuthDto userForAuth)
    {
        var httpResponse = await _client.PostAsJsonAsync("auth/login", userForAuth, _options);
        if (httpResponse?.IsSuccessStatusCode ?? false)
        {
            var jsonStream = await httpResponse.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<AuthResponseDto>(jsonStream, _options);

            await SaveToLocalStorageOrLogout(result, false);

            return true;
        }
        return false;
    }

    public async Task Logout(bool serverLogout = true)
    {
        if (serverLogout)
        {
            await _client.GetAsync("auth/logout");
        }

        await _localStorage.RemoveItemAsync("token");
        ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
    }

    public async Task<string?> RefreshToken()
    {
        var token = await _localStorage.GetItemAsync<AuthResponseDto>("token");

        var httpResponse = await _client.PostAsJsonAsync("auth/refresh", new RefreshTokenDto { Token = token.Token, RefreshToken = token.RefreshToken }, _options);

        if (httpResponse?.IsSuccessStatusCode ?? false)
        {
            var jsonStream = await httpResponse.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<AuthResponseDto>(jsonStream, _options);

            await SaveToLocalStorageOrLogout(result, false);

            return result?.Token;
        }

        return null;
    }

    public async Task<bool> RegisterUser(UserForRegistrationDto userForRegistration)
    {
        var httpResponse = await _client.PostAsJsonAsync("auth/registration", userForRegistration, _options);
        if (httpResponse?.IsSuccessStatusCode ?? false)
        {
            var jsonStream = await httpResponse.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<AuthResponseDto>(jsonStream, _options);

            await SaveToLocalStorageOrLogout(result, false);

            return true;
        }
        return false;
    }

    private async Task SaveToLocalStorageOrLogout(AuthResponseDto? response, bool serverLogout = true)
    {
        if (response is not null && !string.IsNullOrEmpty(response.Token) && !string.IsNullOrEmpty(response.RefreshToken))
        {
            await _localStorage.SetItemAsync("token", response);
            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(response.Token);
        }
        else
        {
            await Logout(serverLogout);
        }
    }
}
