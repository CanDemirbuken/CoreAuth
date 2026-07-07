using CoreAuth.Application.DTO;
using CoreAuth.Application.DTO.Auth;
using CoreAuth.Application.DTO.Token;
using CoreAuth.Application.Interfaces.Identity;
using CoreAuth.Application.Interfaces.Services;
using CoreAuth.Application.Interfaces.Token;
using Microsoft.AspNetCore.Http;

namespace CoreAuth.Application.Services;

public class AuthService(IUserIdentityService userIdentityService, ISignInIdentityService signInIdentityService, IJwtTokenService jwtTokenService) : IAuthService
{
    public async Task<ServiceResult<LoginResponseDto>> LoginAsync(LoginRequestDto request)
    {
        var userResult = await userIdentityService.FindByNameAsync(request.UserName);

        if (!userResult.IsSuccess)
            return ServiceResult<LoginResponseDto>.Fail("User not found!", StatusCodes.Status404NotFound);

        var user = userResult.Data!;

        if (!user.IsActive || user.IsDeleted)
            return ServiceResult<LoginResponseDto>.Fail("User account is not active!", StatusCodes.Status403Forbidden);

        var passwordResult = await signInIdentityService.CheckPasswordSignInAsync(request.UserName, request.Password, lockoutOnFailure: true);

        if (!passwordResult.IsSuccess || !passwordResult.Data!.Succeeded)
            return ServiceResult<LoginResponseDto>.Fail("Invalid credentials!", StatusCodes.Status401Unauthorized);

        var rolesResult = await userIdentityService.GetRolesByUserIdAsync(user.Id);

        var roles = rolesResult.IsSuccess ? rolesResult.Data! : new List<string>();

        var loginResponse = jwtTokenService.GenerateToken(
            new TokenGenerateRequestDto(user.Id, user.UserName, user.Email, roles));

        return ServiceResult<LoginResponseDto>.Success(loginResponse, StatusCodes.Status200OK);
    }

    public async Task<ServiceResult> RegisterAsync(RegisterRequestDto request)
    {
        var userWithEmail = await userIdentityService.FindByEmailAsync(request.Email);

        if (userWithEmail.IsSuccess)
            return ServiceResult.Fail("User already exists with this email!", StatusCodes.Status400BadRequest);

        var userWithUserName = await userIdentityService.FindByNameAsync(request.UserName);

        if (userWithUserName.IsSuccess)
            return ServiceResult.Fail("User already exists with this username!", StatusCodes.Status400BadRequest);

        return await userIdentityService.CreateAsync(request);
    }
}