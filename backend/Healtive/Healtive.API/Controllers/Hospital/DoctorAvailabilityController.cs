using Healtive.Application.DTOs.DoctorAvailability;
using Healtive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Healtive.API.Controllers.Hospital;

[ApiController]
[Route("api/hospital/doctors/{doctorId:guid}/availability")]
[Authorize(Roles = "HospitalAdmin")]
public class DoctorAvailabilityController : ControllerBase
{
    private readonly IDoctorAvailabilityService _service;

    public DoctorAvailabilityController(
        IDoctorAvailabilityService service)
    {
        _service = service;
    }

    // Create availability
    [HttpPost]
    public async Task<IActionResult> Create(
        Guid doctorId,
        [FromBody] CreateDoctorAvailabilityRequest request)
    {
        var response = await _service.CreateAsync(
            doctorId,
            request);

        return Ok(response);
    }

    // Get all availability for a doctor
    [HttpGet]
    public async Task<IActionResult> GetByDoctor(
        Guid doctorId)
    {
        var response = await _service.GetByDoctorAsync(
            doctorId);

        return Ok(response);
    }

    // Update availability
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid doctorId,
        Guid id,
        [FromBody] UpdateDoctorAvailabilityRequest request)
    {
        var response = await _service.UpdateAsync(
            doctorId,
            id,
            request);

        return Ok(response);
    }

    // Delete availability
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid doctorId,
        Guid id)
    {
        var response = await _service.DeleteAsync(
            doctorId,
            id);

        return Ok(response);
    }

    // Activate availability
    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> Activate(
        Guid doctorId,
        Guid id)
    {
        var response = await _service.ActivateAsync(
            doctorId,
            id);

        return Ok(response);
    }

    // Deactivate availability
    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(
        Guid doctorId,
        Guid id)
    {
        var response = await _service.DeactivateAsync(
            doctorId,
            id);

        return Ok(response);
    }
}