using CoreAuth.Application.DTO;
using CoreAuth.Application.DTO.Identity;

namespace CoreAuth.Application.Interfaces.Identity;

public interface IRoleIdentityService
{
    Task<ServiceResult<List<IdentityRoleDto>>> GetRolesAsync();
    Task<ServiceResult<bool>> RoleExistsAsync(string roleName);
    Task<ServiceResult> CreateRoleAsync(string roleName);
    Task<ServiceResult<bool>> IsUserInRoleAsync(string userId, string roleName);
    Task<ServiceResult<IdentityUserRoleDto>> AssignRoleToUserAsync(string userId, string roleName);
}