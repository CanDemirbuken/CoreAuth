namespace CoreAuth.Application.DTO.Auth;

public record LoginResponseDto(string AccessToken, DateTime ExpireDate);