using Healtive.Application.DTOs.Doctor.Diagnosis;
using Healtive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Healtive.API.Controllers.Hospital;

[Route("api/hospital/doctor/diagnoses")]
[ApiController]
[Authorize(Roles = "Doctor")]
public class DoctorDiagnosisController : ControllerBase
{
    private readonly IDiagnosisService _service;

    public DoctorDiagnosisController(IDiagnosisService service)
    {
        _service = service;
    }

    // ============================================================
    // GET DIAGNOSES BY APPOINTMENT
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
    // GET DIAGNOSIS BY ID
    // ============================================================

    [HttpGet("{diagnosisId:guid}")]
    public async Task<IActionResult> GetById(Guid diagnosisId)
    {
        var result = await _service.GetByIdAsync(diagnosisId);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }


    // ============================================================
    // CREATE DIAGNOSIS
    // ============================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateDiagnosisRequest request)
    {
        var result = await _service.CreateAsync(request);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }


    // ============================================================
    // UPDATE DIAGNOSIS
    // ============================================================

    [HttpPut("{diagnosisId:guid}")]
    public async Task<IActionResult> Update(
        Guid diagnosisId,
        [FromBody] UpdateDiagnosisRequest request)
    {
        var result = await _service.UpdateAsync(
            diagnosisId,
            request);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }


    // ============================================================
    // DELETE DIAGNOSIS
    // ============================================================

    [HttpDelete("{diagnosisId:guid}")]
    public async Task<IActionResult> Delete(Guid diagnosisId)
    {
        var result = await _service.DeleteAsync(diagnosisId);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}