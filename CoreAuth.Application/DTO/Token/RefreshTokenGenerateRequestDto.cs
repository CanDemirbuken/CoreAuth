namespace CoreAuth.Application.DTO.Token;

public record RefreshTokenGenerateResponseDto(string Token, DateTime ExpiresDate);