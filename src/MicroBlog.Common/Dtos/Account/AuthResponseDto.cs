namespace MicroBlog.Common.Dtos.Account;

public record AuthResponseDto
{
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}