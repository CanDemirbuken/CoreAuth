using CoreAuth.Application.DTO.Role;
using CoreAuth.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreAuth.API.Controllers;

[Authorize(Roles = "Admin")]
public class RoleController(IRoleService roleService) : BaseController
{
    [HttpGet("roles")]
    public async Task<IActionResult> ListRoles()
    {
        var result = await roleService.ListRolesAsync();
        return CreateActionResult(result);
    }

    [HttpPost("roles")]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequestDto request)
    {
        var result = await roleService.CreateRoleAsync(request);
        return CreateActionResult(result);
    }

    [HttpPost("roles/assign")]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequestDto request)
    {
        var result = await roleService.AssignRoleAsync(request);
        return CreateActionResult(result);
    }
}