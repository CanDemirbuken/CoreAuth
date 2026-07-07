namespace CoreAuth.Application.DTO.Identity;

public record PasswordCheckResultDto(bool Succeeded, bool IsLockedOut, bool IsNotAllowed, bool RequiresTwoFactor);