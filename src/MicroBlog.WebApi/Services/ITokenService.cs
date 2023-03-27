namespace MicroBlog.WebApi.Services;

public interface ITokenService
{
    public int AccessTokenExpiration { get; }
    public int RefreshTokenExpiration { get; }
    public Task<List<Claim>> GetClaims(User user);
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    public Task<string> GenerateToken(User user);
    public string GenerateRefreshToken();
}
