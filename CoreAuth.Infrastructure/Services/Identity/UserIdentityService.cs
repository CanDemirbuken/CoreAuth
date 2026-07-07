using CoreAuth.Application.DTO;
using CoreAuth.Application.DTO.Auth;
using CoreAuth.Application.DTO.Identity;
using CoreAuth.Application.Interfaces.Identity;
using CoreAuth.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CoreAuth.Infrastructure.Services.Identity;

public class UserIdentityService(UserManager<AppUser> userManager) : IUserIdentityService
{
    public async Task<ServiceResult> CreateAsync(RegisterRequestDto request)
    {
        var user = new AppUser
        {
            UserName = request.UserName,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            IsActive = true,
            IsDeleted = false
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return ServiceResult.Fail(result.Errors.Select(e => e.Description).ToList(), StatusCodes.Status400BadRequest);

        return ServiceResult.Success(StatusCodes.Status201Created);
    }

    public async Task<ServiceResult<IdentityUserDto>> FindByIdAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return ServiceResult<IdentityUserDto>.Fail("User not found", StatusCodes.Status404NotFound);

        return ServiceResult<IdentityUserDto>.Success(MapToIdentityUserDto(user), StatusCodes.Status200OK);
    }

    public async Task<ServiceResult<IdentityUserDto>> FindByNameAsync(string userName)
    {
        var user = await userManager.FindByNameAsync(userName);

        if (user is null)
            return ServiceResult<IdentityUserDto>.Fail("User not found", StatusCodes.Status404NotFound);

        return ServiceResult<IdentityUserDto>.Success(MapToIdentityUserDto(user), StatusCodes.Status200OK);
    }

    public async Task<ServiceResult<IdentityUserDto>> FindByEmailAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
            return ServiceResult<IdentityUserDto>.Fail("User not found", StatusCodes.Status404NotFound);

        return ServiceResult<IdentityUserDto>.Success(MapToIdentityUserDto(user), StatusCodes.Status200OK);
    }

    public async Task<ServiceResult<IList<string>>> GetRolesByUserIdAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return ServiceResult<IList<string>>.Fail("User not found", StatusCodes.Status404NotFound);

        var roles = await userManager.GetRolesAsync(user);

        return ServiceResult<IList<string>>.Success(roles, StatusCodes.Status200OK);
    }

    private static IdentityUserDto MapToIdentityUserDto(AppUser user)
    {
        return new IdentityUserDto(
            user.Id,
            user.UserName!,
            user.Email!,
            user.IsActive,
            user.IsDeleted,
            user.EmailConfirmed
        );
    }
}