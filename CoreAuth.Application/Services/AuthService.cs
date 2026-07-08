using CoreAuth.Application.DTO;
using CoreAuth.Application.DTO.Auth;
using CoreAuth.Application.DTO.Token;
using CoreAuth.Application.Interfaces.Identity;
using CoreAuth.Application.Interfaces.Repositories;
using CoreAuth.Application.Interfaces.Services;
using CoreAuth.Application.Interfaces.Token;
using CoreAuth.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace CoreAuth.Application.Services;

public class AuthService(IUserIdentityService userIdentityService, ISignInIdentityService signInIdentityService, IJwtTokenService jwtTokenService, IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork) : IAuthService
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

        var accessTokenResponse = jwtTokenService.GenerateAccessToken(
            new TokenGenerateRequestDto(user.Id, user.UserName, user.Email, roles));

        var refreshTokenResponse = jwtTokenService.GenerateRefreshToken();
        var refreshToken = new RefreshToken
        {
            Token = refreshTokenResponse.Token,
            ExpiresDate = refreshTokenResponse.ExpiresDate,
            CreatedDate = DateTime.UtcNow,
            UserId = user.Id
        };

        await refreshTokenRepository.AddAsync(refreshToken);
        await unitOfWork.SaveChangesAsync();

        var response = new LoginResponseDto(accessTokenResponse.Token, accessTokenResponse.ExpiresDate, refreshTokenResponse.Token, refreshTokenResponse.ExpiresDate);
        return ServiceResult<LoginResponseDto>.Success(response, StatusCodes.Status200OK);
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

    public async Task<ServiceResult<LoginResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        var refreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
        if (refreshToken is null)
            return ServiceResult<LoginResponseDto>.Fail("Invalid refresh token!", StatusCodes.Status401Unauthorized);

        if (!refreshToken.IsActive)
            return ServiceResult<LoginResponseDto>.Fail("Invalid refresh token!", StatusCodes.Status401Unauthorized);

        var userResult = await userIdentityService.FindByIdAsync(refreshToken.UserId);
        if (!userResult.IsSuccess)
            return ServiceResult<LoginResponseDto>.Fail("Invalid refresh token!", StatusCodes.Status401Unauthorized);

        var user = userResult.Data!;
        if (!user.IsActive || user.IsDeleted)
            return ServiceResult<LoginResponseDto>.Fail("Invalid refresh token!", StatusCodes.Status401Unauthorized);

        var rolesResult = await userIdentityService.GetRolesByUserIdAsync(user.Id);
        var roles = rolesResult.IsSuccess ? rolesResult.Data! : new List<string>();

        var accessTokenResponse = jwtTokenService.GenerateAccessToken(new TokenGenerateRequestDto(user.Id, user.UserName!, user.Email!, roles));

        var newRefreshTokenResponse = jwtTokenService.GenerateRefreshToken();
        var newRefreshToken = new RefreshToken
        {
            Token = newRefreshTokenResponse.Token,
            ExpiresDate = newRefreshTokenResponse.ExpiresDate,
            CreatedDate = DateTime.UtcNow,
            UserId = user.Id
        };

        refreshToken.RevokedDate = DateTime.UtcNow;
        refreshToken.ReplacedByToken = newRefreshTokenResponse.Token;

        await refreshTokenRepository.AddAsync(newRefreshToken);
        await unitOfWork.SaveChangesAsync();

        var response = new LoginResponseDto(accessTokenResponse.Token, accessTokenResponse.ExpiresDate, newRefreshTokenResponse.Token, newRefreshTokenResponse.ExpiresDate);
        return ServiceResult<LoginResponseDto>.Success(response, StatusCodes.Status200OK);
    }

    public async Task<ServiceResult> RevokeAsync(RefreshTokenRequestDto request)
    {
        var refreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
        if (refreshToken is null)
            return ServiceResult.Fail("Invalid refresh token!", StatusCodes.Status401Unauthorized);

        if (!refreshToken.IsActive)
            return ServiceResult.Success(StatusCodes.Status200OK);

        var userResult = await userIdentityService.FindByIdAsync(refreshToken.UserId);
        if (!userResult.IsSuccess)
            return ServiceResult.Fail("Invalid refresh token!", StatusCodes.Status401Unauthorized);

        var user = userResult.Data!;
        if (!user.IsActive || user.IsDeleted)
            return ServiceResult.Fail("Invalid refresh token!", StatusCodes.Status401Unauthorized);

        refreshToken.RevokedDate = DateTime.UtcNow;
        await unitOfWork.SaveChangesAsync();

        return ServiceResult.Success(StatusCodes.Status200OK);
    }
}