using Healtive.Application.DTOs.Hospital;
using Healtive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Healtive.API.Controllers;

[Authorize(Roles = "SuperAdmin")]
[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IHospitalService _hospitalService;

    public AdminController(
        IHospitalService hospitalService)
    {
        _hospitalService = hospitalService;
    }

    [HttpPost("hospitals")]
    public async Task<IActionResult> CreateHospital(
        CreateHospitalRequest request)
    {
        var result = await _hospitalService.CreateAsync(request);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("hospitals/{id:guid}")]
    public async Task<IActionResult> GetHospital(Guid id)
    {
        var result = await _hospitalService.GetByIdAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("hospitals")]
    public async Task<IActionResult> GetHospitals()
    {
        var result = await _hospitalService.GetAllAsync();

        return Ok(result);
    }

    [HttpPut("hospitals/{id:guid}")]
    public async Task<IActionResult> UpdateHospital(
    Guid id,
    UpdateHospitalRequest request)
    {
        var result = await _hospitalService.UpdateAsync(id, request);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }
}