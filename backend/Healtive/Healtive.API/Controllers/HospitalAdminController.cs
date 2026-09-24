using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Hospital;
using Healtive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Healtive.API.Controllers;

[Authorize(Roles = "HospitalAdmin")]
[ApiController]
[Route("api/hospital")]
public class HospitalAdminController : ControllerBase
{
    private readonly ICurrentUserService _currentUser;
    private readonly IHospitalService _hospitalService;

    public HospitalAdminController(
        ICurrentUserService currentUser,
        IHospitalService hospitalService)
    {
        _currentUser = currentUser;
        _hospitalService = hospitalService;
    }

    [HttpGet("context")]
    public IActionResult GetContext()
    {
        return Ok(new
        {
            UserId = _currentUser.UserId,
            HospitalId = _currentUser.HospitalId,
            BranchId = _currentUser.BranchId,
            Role = _currentUser.Role
        });
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile(
        CancellationToken cancellationToken)
    {
        var result = await _hospitalService.GetCurrentAsync();

        return Ok(result);
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateHospitalRequest request,
        CancellationToken cancellationToken)
    {
        if (request == null)
        {
            return BadRequest(
                ApiResponse<string>.FailureResponse(
                    "Invalid request."));
        }

        var result = await _hospitalService.UpdateCurrentAsync(request);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}