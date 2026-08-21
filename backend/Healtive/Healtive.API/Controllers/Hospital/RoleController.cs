using Healtive.Application.DTOs.Role;
using Healtive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Healtive.API.Controllers.Hospital;

[ApiController]
[Route("api/hospital/roles")]
[Authorize(Roles = "HospitalAdmin")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateRoleRequest request)
    {
        var response = await _roleService.CreateAsync(request);

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] RoleFilterRequest request)
    {
        var response = await _roleService.GetAllAsync(request);

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id)
    {
        var response = await _roleService.GetByIdAsync(id);

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateRoleRequest request)
    {
        var response = await _roleService.UpdateAsync(
            id,
            request);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id)
    {
        var response = await _roleService.DeleteAsync(id);

        return Ok(response);
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> Activate(
        Guid id)
    {
        var response = await _roleService.ActivateAsync(id);

        return Ok(response);
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(
        Guid id)
    {
        var response = await _roleService.DeactivateAsync(id);

        return Ok(response);
    }
}