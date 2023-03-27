namespace MicroBlog.Common.Dtos.Account;

public record PasswordChangeDto(string OldPassword, string NewPassword, string NewPasswordConfirm);
