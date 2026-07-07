using CoreAuth.Application.DTO;
using CoreAuth.Application.DTO.Identity;
using CoreAuth.Application.Interfaces.Identity;
using CoreAuth.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CoreAuth.Infrastructure.Services.Identity;

public class RoleIdentityService(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager) : IRoleIdentityService
{
    public async Task<ServiceResult<List<IdentityRoleDto>>> GetRolesAsync()
    {
        var roles = await roleManager.Roles
            .Select(role => new IdentityRoleDto(role.Id, role.Name!))
            .ToListAsync();

        return ServiceResult<List<IdentityRoleDto>>.Success(roles, StatusCodes.Status200OK);
    }

    public async Task<ServiceResult<bool>> RoleExistsAsync(string roleName)
    {
        var exists = await roleManager.RoleExistsAsync(roleName);

        return ServiceResult<bool>.Success(exists, StatusCodes.Status200OK);
    }

    public async Task<ServiceResult> CreateRoleAsync(string roleName)
    {
        var result = await roleManager.CreateAsync(new IdentityRole(roleName));

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return ServiceResult.Fail(errors, StatusCodes.Status400BadRequest);
        }

        return ServiceResult.Success(StatusCodes.Status201Created);
    }

    public async Task<ServiceResult<bool>> IsUserInRoleAsync(string userId, string roleName)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return ServiceResult<bool>.Fail("User not found", StatusCodes.Status404NotFound);

        var isInRole = await userManager.IsInRoleAsync(user, roleName);

        return ServiceResult<bool>.Success(isInRole, StatusCodes.Status200OK);
    }

    public async Task<ServiceResult<IdentityUserRoleDto>> AssignRoleToUserAsync(string userId, string roleName)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return ServiceResult<IdentityUserRoleDto>.Fail("User not found", StatusCodes.Status404NotFound);

        var result = await userManager.AddToRoleAsync(user, roleName);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return ServiceResult<IdentityUserRoleDto>.Fail(errors, StatusCodes.Status400BadRequest);
        }

        var response = new IdentityUserRoleDto(
            user.Id,
            user.UserName!,
            roleName
        );

        return ServiceResult<IdentityUserRoleDto>.Success(response, StatusCodes.Status200OK);
    }
}