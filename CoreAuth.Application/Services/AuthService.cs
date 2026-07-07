using CoreAuth.Application.DTO;
using CoreAuth.Application.DTO.Auth;
using CoreAuth.Application.Interfaces.Services;
using CoreAuth.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CoreAuth.Application.Services;

public class AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : IAuthService
{
    public async Task<ServiceResult> LoginAsync(LoginRequestDto request)
    {
        var user = await userManager.FindByNameAsync(request.UserName);
        if (user == null)
            return ServiceResult.Fail("User not found!", StatusCodes.Status404NotFound);

        if (!user.IsActive || user.IsDeleted)
            return ServiceResult.Fail("User account is not active!", StatusCodes.Status403Forbidden);

        //if (!user.EmailConfirmed)
        //    return ServiceResult.Fail("Email address is not confirmed!", StatusCodes.Status403Forbidden);

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
        if (!result.Succeeded)
            return ServiceResult.Fail("Invalid credentials!", StatusCodes.Status401Unauthorized);

        return ServiceResult.Success(StatusCodes.Status200OK);
    }

    public async Task<ServiceResult> RegisterAsync(RegisterRequestDto request)
    {
        var userWithEmail = await userManager.FindByEmailAsync(request.Email);
        if (userWithEmail != null)
            return ServiceResult.Fail("User already exists with this email!", StatusCodes.Status400BadRequest);

        var userWithUserName = await userManager.FindByNameAsync(request.UserName);
        if (userWithUserName != null)
            return ServiceResult.Fail("User already exists with this username!", StatusCodes.Status400BadRequest);

        var user = new AppUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.UserName
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => x.Description).ToList();
            return ServiceResult.Fail(errors, StatusCodes.Status400BadRequest);
        }

        return ServiceResult.Success(StatusCodes.Status201Created);
    }
}