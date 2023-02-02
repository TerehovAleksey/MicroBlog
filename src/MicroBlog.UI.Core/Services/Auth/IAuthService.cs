using MicroBlog.Common.Dtos.Account;

namespace MicroBlog.UI.Core.Services.Auth;

public interface IAuthService
{
    Task<bool> RegisterUser(UserForRegistrationDto userForRegistration);
    Task<bool> Login(UserForAuthDto userForAuth);
    Task Logout(bool serverLogout = true);
    Task<string?> RefreshToken();
    Task<ProfileDto?> GetProfileAsync();
}
