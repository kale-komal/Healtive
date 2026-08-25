using Healtive.Application.DTOs.DoctorLeave;
using Healtive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Healtive.API.Controllers.Hospital;

[ApiController]
[Route("api/hospital/doctors/{doctorId:guid}/leaves")]
[Authorize(Roles = "HospitalAdmin")]
public class DoctorLeaveController : ControllerBase
{
    private readonly IDoctorLeaveService _service;

    public DoctorLeaveController(
        IDoctorLeaveService service)
    {
        _service = service;
    }

    // Create leave
    [HttpPost]
    public async Task<IActionResult> Create(
        Guid doctorId,
        [FromBody] CreateDoctorLeaveRequest request)
    {
        var response = await _service.CreateAsync(
            doctorId,
            request);

        return Ok(response);
    }

    // Get all leaves for doctor
    [HttpGet]
    public async Task<IActionResult> GetByDoctor(
        Guid doctorId)
    {
        var response = await _service.GetByDoctorAsync(
            doctorId);

        return Ok(response);
    }

    // Update leave
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid doctorId,
        Guid id,
        [FromBody] UpdateDoctorLeaveRequest request)
    {
        var response = await _service.UpdateAsync(
            doctorId,
            id,
            request);

        return Ok(response);
    }

    // Delete leave
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

    // Approve leave
    [HttpPatch("{id:guid}/approve")]
    public async Task<IActionResult> Approve(
        Guid doctorId,
        Guid id)
    {
        var response = await _service.ApproveAsync(
            doctorId,
            id);

        return Ok(response);
    }

    // Reject leave
    [HttpPatch("{id:guid}/reject")]
    public async Task<IActionResult> Reject(
        Guid doctorId,
        Guid id)
    {
        var response = await _service.RejectAsync(
            doctorId,
            id);

        return Ok(response);
    }
}