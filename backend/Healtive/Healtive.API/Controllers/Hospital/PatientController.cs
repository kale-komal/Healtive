using Healtive.Application.DTOs.Patient;
using Healtive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Healtive.API.Controllers.Hospital;

[ApiController]
[Route("api/hospital/patients")]
[Authorize(Roles = "HospitalAdmin,Receptionist,Doctor")]
public class PatientController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientController(
        IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreatePatientRequest request)
    {
        var response =
            await _patientService.CreateAsync(request);

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] PatientFilterRequest request)
    {
        var response =
            await _patientService.GetAllAsync(request);

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id)
    {
        var response =
            await _patientService.GetByIdAsync(id);

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdatePatientRequest request)
    {
        var response =
            await _patientService.UpdateAsync(
                id,
                request);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id)
    {
        var response =
            await _patientService.DeleteAsync(id);

        return Ok(response);
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> Activate(
        Guid id)
    {
        var response =
            await _patientService.ActivateAsync(id);

        return Ok(response);
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(
        Guid id)
    {
        var response =
            await _patientService.DeactivateAsync(id);

        return Ok(response);
    }
}