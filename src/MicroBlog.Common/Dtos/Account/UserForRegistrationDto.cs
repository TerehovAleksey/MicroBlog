namespace MicroBlog.Common.Dtos.Account;

public record UserForRegistrationDto
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
}
