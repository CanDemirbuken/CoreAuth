using CoreAuth.Application.DTO;
using CoreAuth.Application.DTO.Identity;

namespace CoreAuth.Application.Interfaces.Identity;

public interface ISignInIdentityService
{
    Task<ServiceResult<PasswordCheckResultDto>> CheckPasswordSignInAsync(string userName, string password, bool lockoutOnFailure);
}