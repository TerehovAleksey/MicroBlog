namespace MicroBlog.Common.Dtos.Account;

public record UserForAuthDto
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}
