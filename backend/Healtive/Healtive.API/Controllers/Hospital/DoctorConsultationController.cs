using Healtive.Application.DTOs.Doctor.Consultation;
using Healtive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Healtive.API.Controllers.Hospital;

[Route("api/hospital/doctor/consultations")]
[ApiController]
[Authorize(Roles = "Doctor")]
public class DoctorConsultationController : ControllerBase
{
    private readonly IConsultationService _service;

    public DoctorConsultationController(
        IConsultationService service)
    {
        _service = service;
    }

    // =========================================================
    // CREATE CONSULTATION
    // =========================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateConsultationRequest request)
    {
        var result =
            await _service.CreateAsync(request);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    // =========================================================
    // GET CONSULTATION BY APPOINTMENT
    // =========================================================

    [HttpGet("appointment/{appointmentId:guid}")]
    public async Task<IActionResult> GetByAppointment(
        Guid appointmentId)
    {
        var result =
            await _service.GetByAppointmentIdAsync(
                appointmentId);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    // =========================================================
    // UPDATE CONSULTATION
    // =========================================================

    [HttpPut("{consultationId:guid}")]
    public async Task<IActionResult> Update(
        Guid consultationId,
        [FromBody] UpdateConsultationRequest request)
    {
        var result =
            await _service.UpdateAsync(
                consultationId,
                request);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    // =========================================================
    // COMPLETE CONSULTATION
    // =========================================================

    [HttpPut("{consultationId:guid}/complete")]
    public async Task<IActionResult> Complete(
        Guid consultationId)
    {
        var result =
            await _service.CompleteAsync(
                consultationId);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}