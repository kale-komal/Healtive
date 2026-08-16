using Healtive.Application.DTOs.Department;
using Healtive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Healtive.API.Controllers.Hospital;

[ApiController]
[Route("api/hospital/departments")]
[Authorize(Roles = "HospitalAdmin")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(
        IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search = null,
        [FromQuery] bool? status = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var response = await _departmentService.GetAllAsync(
            search,
            status,
            page,
            pageSize);

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id)
    {
        var response = await _departmentService.GetByIdAsync(id);

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateDepartmentRequest request)
    {
        var response = await _departmentService.CreateAsync(request);

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateDepartmentRequest request)
    {
        var response = await _departmentService.UpdateAsync(
            id,
            request);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id)
    {
        var response = await _departmentService.DeleteAsync(id);

        return Ok(response);
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> Activate(
        Guid id)
    {
        var response = await _departmentService.ActivateAsync(id);

        return Ok(response);
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(
        Guid id)
    {
        var response = await _departmentService.DeactivateAsync(id);

        return Ok(response);
    }
}