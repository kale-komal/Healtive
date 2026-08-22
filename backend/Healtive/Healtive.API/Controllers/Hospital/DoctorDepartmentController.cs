using Healtive.Application.DTOs.Doctor;
using Healtive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Healtive.API.Controllers.Hospital;

[ApiController]
[Route("api/hospital")]
[Authorize(Roles = "HospitalAdmin")]
public class DoctorDepartmentController : ControllerBase
{
    private readonly IDoctorDepartmentService _service;

    public DoctorDepartmentController(
        IDoctorDepartmentService service)
    {
        _service = service;
    }

    // Assign department to doctor
    [HttpPost("doctors/{doctorId:guid}/departments")]
    public async Task<IActionResult> Assign(
        Guid doctorId,
        [FromBody] AssignDoctorDepartmentRequest request)
    {
        var response = await _service.AssignAsync(
            doctorId,
            request);

        return Ok(response);
    }

    // Get departments assigned to doctor
    [HttpGet("doctors/{doctorId:guid}/departments")]
    public async Task<IActionResult> GetDoctorDepartments(
        Guid doctorId)
    {
        var response =
            await _service.GetDoctorDepartmentsAsync(
                doctorId);

        return Ok(response);
    }

    // Remove department from doctor
    [HttpDelete(
        "doctors/{doctorId:guid}/departments/{departmentId:guid}")]
    public async Task<IActionResult> Remove(
        Guid doctorId,
        Guid departmentId)
    {
        var response = await _service.RemoveAsync(
            doctorId,
            departmentId);

        return Ok(response);
    }

    // Get doctors assigned to department
    [HttpGet("departments/{departmentId:guid}/doctors")]
    public async Task<IActionResult> GetDepartmentDoctors(
        Guid departmentId)
    {
        var response =
            await _service.GetDepartmentDoctorsAsync(
                departmentId);

        return Ok(response);
    }
}