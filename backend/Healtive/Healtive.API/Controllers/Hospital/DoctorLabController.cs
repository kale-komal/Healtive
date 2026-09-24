using Healtive.Application.DTOs.Doctor.Lab;
using Healtive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Healtive.API.Controllers.Hospital;

[Route("api/hospital/doctor/lab")]
[ApiController]
[Authorize(Roles = "Doctor")]
public class DoctorLabController : ControllerBase
{
    private readonly IDoctorLabService _service;

    public DoctorLabController(IDoctorLabService service)
    {
        _service = service;
    }

    // ============================================================
    // GET ACTIVE LAB CATEGORIES
    // ============================================================

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var result = await _service.GetActiveCategoriesAsync();

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }


    // ============================================================
    // GET ACTIVE LAB TESTS
    // ============================================================

    [HttpGet("tests")]
    public async Task<IActionResult> GetTests(
        [FromQuery] Guid? categoryId)
    {
        var result = await _service.GetActiveTestsAsync(categoryId);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }


    // ============================================================
    // GET LAB ORDERS BY APPOINTMENT
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
    // GET LAB ORDER BY ID
    // ============================================================

    [HttpGet("{orderId:guid}")]
    public async Task<IActionResult> GetById(Guid orderId)
    {
        var result = await _service.GetByIdAsync(orderId);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }


    // ============================================================
    // CREATE LAB ORDERS
    // ============================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateLabOrderRequest request)
    {
        var result = await _service.CreateAsync(request);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }


    // ============================================================
    // UPDATE LAB ORDER
    // ============================================================

    [HttpPut("{orderId:guid}")]
    public async Task<IActionResult> Update(
        Guid orderId,
        [FromBody] UpdateLabOrderRequest request)
    {
        var result = await _service.UpdateAsync(
            orderId,
            request);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }


    // ============================================================
    // CANCEL LAB ORDER
    // ============================================================

    [HttpPut("{orderId:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid orderId)
    {
        var result = await _service.CancelAsync(orderId);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}