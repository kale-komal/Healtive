using Healtive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Healtive.API.Controllers.Hospital;

[ApiController]
[Route("api/hospital/doctor/patients")]
[Authorize(Roles = "Doctor")]
public class DoctorPatientController : ControllerBase
{
    private readonly IDoctorPatientService _doctorPatientService;

    public DoctorPatientController(
        IDoctorPatientService doctorPatientService)
    {
        _doctorPatientService = doctorPatientService;
    }

    // =========================================================
    // GET PATIENT PROFILE
    // =========================================================

    [HttpGet("{patientId:guid}")]
    public async Task<IActionResult> GetPatientProfile(
        Guid patientId)
    {
        var response =
            await _doctorPatientService
                .GetPatientProfileAsync(patientId);

        if (response == null)
        {
            return NotFound(new
            {
                message =
                    "Patient not found or patient is not associated with this doctor."
            });
        }

        return Ok(response);
    }
}