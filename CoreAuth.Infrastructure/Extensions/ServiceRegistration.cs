using CoreAuth.Application.Interfaces.Identity;
using CoreAuth.Application.Interfaces.Token;
using CoreAuth.Infrastructure.Options;
using CoreAuth.Infrastructure.Services.Identity;
using CoreAuth.Infrastructure.Services.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CoreAuth.Infrastructure.Extensions;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings!.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
            };
        });

        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IUserIdentityService, UserIdentityService>();
        services.AddScoped<ISignInIdentityService, SignInIdentityService>();
        services.AddScoped<IRoleIdentityService, RoleIdentityService>();

        return services;
    }
}