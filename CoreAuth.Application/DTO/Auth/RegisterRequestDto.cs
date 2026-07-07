namespace CoreAuth.Application.DTO.Auth;

public record RegisterRequestDto(string FirstName, string LastName, string Email, string UserName, string Password, string ConfirmPassword);