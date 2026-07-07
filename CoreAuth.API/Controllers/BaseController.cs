using CoreAuth.Application.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CoreAuth.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    protected IActionResult CreateActionResult(ServiceResult result)
    {
        if (result.IsSuccess)
            return StatusCode(result.StatusCode);

        return StatusCode(result.StatusCode, result);
    }

    protected IActionResult CreateActionResult<T>(ServiceResult<T> result)
    {
        if (result.IsSuccess)
            return StatusCode(result.StatusCode, result.Data);

        return StatusCode(result.StatusCode, result);
    }
}