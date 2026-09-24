using Healtive.Application.DTOs.Doctor.FollowUp;
using Healtive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Healtive.API.Controllers.Hospital;

[Route("api/hospital/doctor/followups")]
[ApiController]
[Authorize(Roles = "Doctor")]
public class DoctorFollowUpController : ControllerBase
{
    private readonly IDoctorFollowUpService _service;

    public DoctorFollowUpController(IDoctorFollowUpService service)
    {
        _service = service;
    }

    // ============================================================
    // GET UPCOMING FOLLOW-UPS
    // ============================================================

    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcoming()
    {
        var result = await _service.GetUpcomingAsync();

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }


    // ============================================================
    // GET FOLLOW-UPS BY APPOINTMENT
    // ============================================================

    [HttpGet("appointment/{appointmentId:guid}")]
    public async Task<IActionResult> GetByAppointment(Guid appointmentId)
    {
        var result = await _service.GetByAppointmentIdAsync(appointmentId);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }


    // ============================================================
    // GET FOLLOW-UP BY ID
    // ============================================================

    [HttpGet("{followUpId:guid}")]
    public async Task<IActionResult> GetById(Guid followUpId)
    {
        var result = await _service.GetByIdAsync(followUpId);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }


    // ============================================================
    // CREATE FOLLOW-UP
    // ============================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateFollowUpRequest request)
    {
        var result = await _service.CreateAsync(request);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }


    // ============================================================
    // UPDATE FOLLOW-UP
    // ============================================================

    [HttpPut("{followUpId:guid}")]
    public async Task<IActionResult> Update(
        Guid followUpId,
        [FromBody] UpdateFollowUpRequest request)
    {
        var result = await _service.UpdateAsync(
            followUpId,
            request);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }


    // ============================================================
    // COMPLETE FOLLOW-UP
    // ============================================================

    [HttpPut("{followUpId:guid}/complete")]
    public async Task<IActionResult> Complete(Guid followUpId)
    {
        var result = await _service.CompleteAsync(followUpId);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }


    // ============================================================
    // CANCEL FOLLOW-UP
    // ============================================================

    [HttpPut("{followUpId:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid followUpId)
    {
        var result = await _service.CancelAsync(followUpId);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}