using Healtive.Application.DTOs.Doctor.MedicalHistory;
using Healtive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Healtive.API.Controllers.Hospital;

[Route("api/hospital/doctor/medical-history")]
[ApiController]
[Authorize(Roles = "Doctor")]
public class DoctorMedicalHistoryController : ControllerBase
{
    private readonly IMedicalHistoryService _service;

    public DoctorMedicalHistoryController(
        IMedicalHistoryService service)
    {
        _service = service;
    }

    // ==========================================
    // GET PATIENT MEDICAL HISTORY
    // ==========================================

    [HttpGet("patient/{patientId:guid}")]
    public async Task<IActionResult> GetByPatient(
        Guid patientId)
    {
        var result = await _service.GetByPatientIdAsync(patientId);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }


    // ==========================================
    // GET MEDICAL HISTORY BY ID
    // ==========================================

    [HttpGet("{historyId:guid}")]
    public async Task<IActionResult> GetById(
        Guid historyId)
    {
        var result = await _service.GetByIdAsync(historyId);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }


    // ==========================================
    // CREATE MEDICAL HISTORY
    // ==========================================

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateMedicalHistoryRequest request)
    {
        var result = await _service.CreateAsync(request);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }


    // ==========================================
    // UPDATE MEDICAL HISTORY
    // ==========================================

    [HttpPut("{historyId:guid}")]
    public async Task<IActionResult> Update(
        Guid historyId,
        [FromBody] UpdateMedicalHistoryRequest request)
    {
        var result = await _service.UpdateAsync(
            historyId,
            request);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }


    // ==========================================
    // DELETE MEDICAL HISTORY
    // ==========================================

    [HttpDelete("{historyId:guid}")]
    public async Task<IActionResult> Delete(
        Guid historyId)
    {
        var result = await _service.DeleteAsync(historyId);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}