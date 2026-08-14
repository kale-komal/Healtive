using Healtive.Application.DTOs.Branch;
using Healtive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Healtive.API.Controllers;

[Authorize(Roles = "HospitalAdmin")]
[ApiController]
[Route("api/hospital/branches")]
public class BranchController : ControllerBase
{
    private readonly IBranchService _branchService;

    public BranchController(
        IBranchService branchService)
    {
        _branchService = branchService;
    }

    // GET: api/hospital/branches
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search = null,
        [FromQuery] bool? status = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _branchService.GetAllAsync(
            search,
            status,
            page,
            pageSize);

        return Ok(result);
    }

    // GET: api/hospital/branches/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _branchService.GetByIdAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    // POST: api/hospital/branches
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateBranchRequest request)
    {
        var result = await _branchService.CreateAsync(request);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    // PUT: api/hospital/branches/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateBranchRequest request)
    {
        var result = await _branchService.UpdateAsync(
            id,
            request);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    // DELETE: api/hospital/branches/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _branchService.DeleteAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    // PATCH: api/hospital/branches/{id}/activate
    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        var result = await _branchService.ActivateAsync(id);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    // PATCH: api/hospital/branches/{id}/deactivate
    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var result = await _branchService.DeactivateAsync(id);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}