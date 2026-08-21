using Healtive.Application.DTOs.DoctorSpecialization;
using Healtive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Healtive.API.Controllers.Hospital;

[ApiController]
[Route("api/hospital/doctor-specializations")]
[Authorize(Roles = "HospitalAdmin")]
public class DoctorSpecializationController : ControllerBase
{
    private readonly IDoctorSpecializationService _service;

    public DoctorSpecializationController(
        IDoctorSpecializationService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateDoctorSpecializationRequest request)
    {
        var response = await _service.CreateAsync(request);

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] DoctorSpecializationFilterRequest request)
    {
        var response = await _service.GetAllAsync(request);

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _service.GetByIdAsync(id);

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateDoctorSpecializationRequest request)
    {
        var response = await _service.UpdateAsync(
            id,
            request);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _service.DeleteAsync(id);

        return Ok(response);
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        var response = await _service.ActivateAsync(id);

        return Ok(response);
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var response = await _service.DeactivateAsync(id);

        return Ok(response);
    }
}