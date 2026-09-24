using Healtive.Application.DTOs.Doctor.Prescription;
using Healtive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Healtive.API.Controllers.Hospital;

[Route("api/hospital/doctor/prescriptions")]
[ApiController]
[Authorize(Roles = "Doctor")]
public class DoctorPrescriptionController : ControllerBase
{
    private readonly IDoctorPrescriptionService _service;

    public DoctorPrescriptionController(IDoctorPrescriptionService service)
    {
        _service = service;
    }

    // ============================================================
    // GET PRESCRIPTIONS BY APPOINTMENT
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
    // GET PRESCRIPTION BY ID
    // ============================================================

    [HttpGet("{prescriptionId:guid}")]
    public async Task<IActionResult> GetById(Guid prescriptionId)
    {
        var result = await _service.GetByIdAsync(prescriptionId);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }


    // ============================================================
    // CREATE PRESCRIPTION
    // ============================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreatePrescriptionRequest request)
    {
        var result = await _service.CreateAsync(request);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }


    // ============================================================
    // UPDATE PRESCRIPTION
    // ============================================================

    [HttpPut("{prescriptionId:guid}")]
    public async Task<IActionResult> Update(
        Guid prescriptionId,
        [FromBody] UpdatePrescriptionRequest request)
    {
        var result = await _service.UpdateAsync(
            prescriptionId,
            request);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }


    // ============================================================
    // DELETE PRESCRIPTION
    // ============================================================

    [HttpDelete("{prescriptionId:guid}")]
    public async Task<IActionResult> Delete(Guid prescriptionId)
    {
        var result = await _service.DeleteAsync(prescriptionId);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }


    // ============================================================
    // FINALIZE PRESCRIPTION
    // ============================================================

    [HttpPut("{prescriptionId:guid}/finalize")]
    public async Task<IActionResult> Finalize(Guid prescriptionId)
    {
        var result = await _service.FinalizeAsync(prescriptionId);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}