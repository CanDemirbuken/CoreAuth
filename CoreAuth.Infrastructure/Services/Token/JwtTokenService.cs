using CoreAuth.Application.DTO.Token;
using CoreAuth.Application.Interfaces.Token;
using CoreAuth.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CoreAuth.Infrastructure.Services.Token
{
    public class JwtTokenService(IOptions<JwtSettings> options, IOptions<RefreshTokenSettings> refreshTokenOptions) : IJwtTokenService
    {
        private readonly JwtSettings jwtSettings = options.Value;
        private readonly RefreshTokenSettings refreshTokenSettings = refreshTokenOptions.Value;

        public AccessTokenGenerateResponseDto GenerateAccessToken(TokenGenerateRequestDto request)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, request.UserId),
                new Claim(ClaimTypes.NameIdentifier, request.UserId),
                new Claim(JwtRegisteredClaimNames.Email, request.Email),
                new Claim(JwtRegisteredClaimNames.UniqueName, request.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var roles = request.Roles ?? new List<string>();
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var expireDate = DateTime.UtcNow.AddMinutes(jwtSettings.ExpireMinutes);

            var token = new JwtSecurityToken
                (
                    issuer: jwtSettings.Issuer,
                    audience: jwtSettings.Audience,
                    claims: claims,
                    expires: expireDate,
                    signingCredentials: credentials
                );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return new AccessTokenGenerateResponseDto(accessToken, expireDate);
        }

        public RefreshTokenGenerateResponseDto GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            var refreshToken = Convert.ToBase64String(randomBytes);

            var expireDate = DateTime.UtcNow.AddDays(refreshTokenSettings.ExpireDays);

            return new RefreshTokenGenerateResponseDto(refreshToken, expireDate);
        }
    }
}