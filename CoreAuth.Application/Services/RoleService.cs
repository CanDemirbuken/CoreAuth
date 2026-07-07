using CoreAuth.Application.DTO;
using CoreAuth.Application.DTO.Role;
using CoreAuth.Application.Interfaces.Identity;
using CoreAuth.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace CoreAuth.Application.Services;

public class RoleService(IRoleIdentityService roleIdentityService) : IRoleService
{
    public async Task<ServiceResult<List<RoleResponseDto>>> ListRolesAsync()
    {
        var rolesResult = await roleIdentityService.GetRolesAsync();

        if (!rolesResult.IsSuccess)
            return ServiceResult<List<RoleResponseDto>>.Fail(rolesResult.Errors!, rolesResult.StatusCode);

        var response = rolesResult.Data!
            .Select(role => new RoleResponseDto(role.Id, role.Name))
            .ToList();

        return ServiceResult<List<RoleResponseDto>>.Success(response, StatusCodes.Status200OK);
    }

    public async Task<ServiceResult> CreateRoleAsync(CreateRoleRequestDto request)
    {
        if (request is null)
            return ServiceResult.Fail("Request cannot be null", StatusCodes.Status400BadRequest);

        if (string.IsNullOrWhiteSpace(request.Name))
            return ServiceResult.Fail("Role name cannot be empty", StatusCodes.Status400BadRequest);

        var roleName = request.Name.Trim();

        var roleExistsResult = await roleIdentityService.RoleExistsAsync(roleName);

        if (!roleExistsResult.IsSuccess)
            return ServiceResult.Fail(roleExistsResult.Errors!, roleExistsResult.StatusCode);

        if (roleExistsResult.Data)
            return ServiceResult.Fail("Role already exists", StatusCodes.Status400BadRequest);

        return await roleIdentityService.CreateRoleAsync(roleName);
    }

    public async Task<ServiceResult<UserRoleResponseDto>> AssignRoleAsync(AssignRoleRequestDto request)
    {
        if (request is null)
            return ServiceResult<UserRoleResponseDto>.Fail("Request cannot be null", StatusCodes.Status400BadRequest);

        if (string.IsNullOrWhiteSpace(request.UserId))
            return ServiceResult<UserRoleResponseDto>.Fail("UserId cannot be empty", StatusCodes.Status400BadRequest);

        if (string.IsNullOrWhiteSpace(request.RoleName))
            return ServiceResult<UserRoleResponseDto>.Fail("RoleName cannot be empty", StatusCodes.Status400BadRequest);

        var roleName = request.RoleName.Trim();

        var roleExistsResult = await roleIdentityService.RoleExistsAsync(roleName);

        if (!roleExistsResult.IsSuccess)
            return ServiceResult<UserRoleResponseDto>.Fail(roleExistsResult.Errors!, roleExistsResult.StatusCode);

        if (!roleExistsResult.Data)
            return ServiceResult<UserRoleResponseDto>.Fail("Role not found", StatusCodes.Status404NotFound);

        var isUserInRoleResult = await roleIdentityService.IsUserInRoleAsync(request.UserId, roleName);

        if (!isUserInRoleResult.IsSuccess)
            return ServiceResult<UserRoleResponseDto>.Fail(isUserInRoleResult.Errors!, isUserInRoleResult.StatusCode);

        if (isUserInRoleResult.Data)
            return ServiceResult<UserRoleResponseDto>.Fail("User already has this role", StatusCodes.Status400BadRequest);

        var assignResult = await roleIdentityService.AssignRoleToUserAsync(request.UserId, roleName);

        if (!assignResult.IsSuccess)
            return ServiceResult<UserRoleResponseDto>.Fail(assignResult.Errors!, assignResult.StatusCode);

        var identityUserRole = assignResult.Data!;

        var response = new UserRoleResponseDto(
            identityUserRole.UserId,
            identityUserRole.UserName,
            identityUserRole.RoleName
        );

        return ServiceResult<UserRoleResponseDto>.Success(response, StatusCodes.Status200OK);
    }
}