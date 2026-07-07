using CoreAuth.Application.DTO;
using CoreAuth.Application.DTO.Auth;

namespace CoreAuth.Application.Interfaces.Services;

public interface IAuthService
{
    Task<ServiceResult> RegisterAsync(RegisterRequestDto request);
    Task<ServiceResult<LoginResponseDto>> LoginAsync(LoginRequestDto request);
}