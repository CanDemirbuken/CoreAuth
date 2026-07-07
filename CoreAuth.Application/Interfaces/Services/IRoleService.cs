using CoreAuth.Application.DTO;
using CoreAuth.Application.DTO.Role;

namespace CoreAuth.Application.Interfaces.Services;

public interface IRoleService
{
    Task<ServiceResult<List<RoleResponseDto>>> ListRolesAsync();
    Task<ServiceResult> CreateRoleAsync(CreateRoleRequestDto request);
    Task<ServiceResult<UserRoleResponseDto>> AssignRoleAsync(AssignRoleRequestDto request);
}