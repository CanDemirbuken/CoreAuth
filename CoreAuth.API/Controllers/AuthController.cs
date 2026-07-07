using CoreAuth.Application.DTO.Auth;
using CoreAuth.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoreAuth.API.Controllers
{
    public class AuthController(IAuthService authService) : BaseController
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            var result = await authService.RegisterAsync(request);
            return CreateActionResult(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var result = await authService.LoginAsync(request);
            return CreateActionResult(result);
        }
    }
}
