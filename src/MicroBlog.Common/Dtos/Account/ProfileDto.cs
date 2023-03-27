namespace MicroBlog.Common.Dtos.Account;

public record ProfileDto
{
    public string? Username { get; set; }
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public string Email { get; set; } = default!;
    public string? Phone { get; set; }
    public string? About { get; set; }
    public string? LogoUri { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public bool IsPhoneConfirmed { get; set; }
}
