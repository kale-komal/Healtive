using Healtive.Application.DTOs.Doctor;
using Healtive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Healtive.API.Controllers.Hospital;

[ApiController]
[Route("api/hospital/doctors")]
[Authorize(Roles = "HospitalAdmin")]
public class DoctorController : ControllerBase
{
    private readonly IDoctorService _service;

    public DoctorController(IDoctorService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateDoctorRequest request)
    {
        var response = await _service.CreateAsync(request);

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] DoctorFilterRequest request)
    {
        var response = await _service.GetAllAsync(request);

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _service.GetByIdAsync(id);

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateDoctorRequest request)
    {
        var response = await _service.UpdateAsync(
            id,
            request);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _service.DeleteAsync(id);

        return Ok(response);
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        var response = await _service.ActivateAsync(id);

        return Ok(response);
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var response = await _service.DeactivateAsync(id);

        return Ok(response);
    }
}