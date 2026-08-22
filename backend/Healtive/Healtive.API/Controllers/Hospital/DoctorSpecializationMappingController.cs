using Healtive.Application.DTOs.Doctor;
using Healtive.Application.DTOs.DoctorSpecialization;
using Healtive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Healtive.API.Controllers.Hospital;

[ApiController]
[Route("api/hospital/doctor-specializations")]
[Authorize(Roles = "HospitalAdmin")]
public class DoctorSpecializationMappingController : ControllerBase
{
    private readonly IDoctorSpecializationMappingService _service;

    public DoctorSpecializationMappingController(
        IDoctorSpecializationMappingService service)
    {
        _service = service;
    }

    // Assign specialization to doctor
    [HttpPost("{doctorId:guid}")]
    public async Task<IActionResult> Assign(
        Guid doctorId,
        [FromBody] AssignDoctorSpecializationRequest request)
    {
        var response = await _service.AssignAsync(
            doctorId,
            request);

        return Ok(response);
    }

    // Get all specializations assigned to doctor
    [HttpGet("doctor/{doctorId:guid}")]
    public async Task<IActionResult> GetDoctorSpecializations(
        Guid doctorId)
    {
        var response =
            await _service.GetDoctorSpecializationsAsync(
                doctorId);

        return Ok(response);
    }

    // Remove specialization from doctor
    [HttpDelete("{doctorId:guid}/{specializationId:guid}")]
    public async Task<IActionResult> Remove(
        Guid doctorId,
        Guid specializationId)
    {
        var response = await _service.RemoveAsync(
            doctorId,
            specializationId);

        return Ok(response);
    }

    // Get all doctors having a specialization
    [HttpGet("specialization/{specializationId:guid}/doctors")]
    public async Task<IActionResult> GetSpecializationDoctors(
        Guid specializationId)
    {
        var response =
            await _service.GetSpecializationDoctorsAsync(
                specializationId);

        return Ok(response);
    }
}