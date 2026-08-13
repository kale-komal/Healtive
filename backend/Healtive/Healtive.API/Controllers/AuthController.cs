using Healtive.Application.DTOs.Auth;
using Healtive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Healtive.Application.DTOs.Common;

namespace Healtive.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        if (!result.Success)
        {
            return Unauthorized(result);
        }

        return Ok(result);
    }


    [Authorize]
    [HttpPatch("change-password")]
    public async Task<IActionResult> ChangePassword(
    [FromBody] ChangePasswordRequest request)
    {
        var userIdClaim =
            User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized(
                ApiResponse<bool>.FailureResponse(
                    "User authentication information not found."));
        }

        if (!Guid.TryParse(
            userIdClaim.Value,
            out var userId))
        {
            return Unauthorized(
                ApiResponse<bool>.FailureResponse(
                    "Invalid user ID."));
        }

        var response =
            await _authService.ChangePasswordAsync(
                userId,
                request);

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
}