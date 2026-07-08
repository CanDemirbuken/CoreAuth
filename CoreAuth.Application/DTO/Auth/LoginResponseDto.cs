namespace CoreAuth.Application.DTO.Auth;

public record LoginResponseDto(string AccessToken, DateTime AccessTokenExpireDate, string RefreshToken, DateTime RefreshTokenExpiresDate);