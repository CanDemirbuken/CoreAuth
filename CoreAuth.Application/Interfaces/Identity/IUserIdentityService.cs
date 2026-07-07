using CoreAuth.Application.DTO;
using CoreAuth.Application.DTO.Auth;
using CoreAuth.Application.DTO.Identity;

namespace CoreAuth.Application.Interfaces.Identity;

public interface IUserIdentityService
{
    Task<ServiceResult<IdentityUserDto>> FindByIdAsync(string userId);
    Task<ServiceResult<IdentityUserDto>> FindByNameAsync(string userName);
    Task<ServiceResult<IdentityUserDto>> FindByEmailAsync(string email);
    Task<ServiceResult<IList<string>>> GetRolesByUserIdAsync(string userId);
    Task<ServiceResult> CreateAsync(RegisterRequestDto request);
}