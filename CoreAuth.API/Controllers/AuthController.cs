using CoreAuth.Application.DTO.Auth;
using CoreAuth.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var result = await authService.RefreshTokenAsync(request);
            return CreateActionResult(result);
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> RevokeAsync(RefreshTokenRequestDto request)
        {
            var result = await authService.RevokeAsync(request);
            return CreateActionResult(result);
        }

        [Authorize]
        [HttpGet("test")]
        public IActionResult Test()
        {
            var userId = User.FindFirst("sub")?.Value; // Get the user ID from the JWT token claims
            var userName = User.FindFirst("nameidentifier")?.Value; // Get the username from the JWT token claims
            var userEmail = User.FindFirst("email")?.Value; // Get the email from the JWT token claims

            return Ok("You are authorized!");
        }
    }
}
