using CoreAuth.Application.Interfaces.Services;
using CoreAuth.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CoreAuth.Application.Extensions;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRoleService, RoleService>();

        return services;
    }
}