using Healtive.Application.DTOs.Doctor.PatientMedicalHistory;
using Healtive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Healtive.API.Controllers.Hospital;

[Route("api/doctor/patients")]
[ApiController]
[Authorize(Roles = "Doctor")]
public class DoctorPatientMedicalHistoryController : ControllerBase
{
    private readonly IDoctorPatientMedicalHistoryService _service;

    public DoctorPatientMedicalHistoryController(
        IDoctorPatientMedicalHistoryService service)
    {
        _service = service;
    }

    // =========================================================
    // GET PATIENT MEDICAL HISTORY
    // =========================================================

    [HttpGet("{patientId:guid}/medical-history")]
    public async Task<IActionResult> GetMedicalHistory(
        Guid patientId,
        [FromQuery] DoctorPatientMedicalHistoryFilterRequest request)
    {
        var result =
            await _service.GetMedicalHistoryAsync(
                patientId,
                request);

        if (result == null)
        {
            return NotFound(new
            {
                message =
                    "Patient not found or patient is not associated with this doctor."
            });
        }

        return Ok(result);
    }
} 