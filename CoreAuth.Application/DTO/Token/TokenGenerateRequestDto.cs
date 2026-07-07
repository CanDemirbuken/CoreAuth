namespace CoreAuth.Application.DTO.Token;

public record TokenGenerateRequestDto(string UserId, string UserName, string Email, IList<string> Roles);