using CoreAuth.Application.DTO;
using CoreAuth.Application.DTO.Identity;
using CoreAuth.Application.Interfaces.Identity;
using CoreAuth.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CoreAuth.Infrastructure.Services.Identity;

public class SignInIdentityService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : ISignInIdentityService
{
    public async Task<ServiceResult<PasswordCheckResultDto>> CheckPasswordSignInAsync(string userName, string password, bool lockoutOnFailure)
    {
        var user = await userManager.FindByNameAsync(userName);

        if (user is null)
            return ServiceResult<PasswordCheckResultDto>.Fail("User not found", StatusCodes.Status404NotFound);

        var result = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure);

        var response = new PasswordCheckResultDto(result.Succeeded, result.IsLockedOut, result.IsNotAllowed, result.RequiresTwoFactor);

        return ServiceResult<PasswordCheckResultDto>.Success(response, StatusCodes.Status200OK);
    }
}