using CoreAuth.Application.DTO.Auth;
using CoreAuth.Application.DTO.Token;

namespace CoreAuth.Application.Interfaces.Token;

public interface IJwtTokenService
{
    LoginResponseDto GenerateToken(TokenGenerateRequestDto request);
}