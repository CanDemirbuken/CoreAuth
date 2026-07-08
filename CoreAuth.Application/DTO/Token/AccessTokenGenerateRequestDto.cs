namespace CoreAuth.Application.DTO.Token;

public record AccessTokenGenerateResponseDto(string Token, DateTime ExpiresDate);