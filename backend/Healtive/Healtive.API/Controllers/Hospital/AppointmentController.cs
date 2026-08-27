using Healtive.Application.DTOs.Appointment;
using Healtive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Healtive.API.Controllers.Hospital;

[ApiController]
[Route("api/hospital/appointments")]
[Authorize(Roles = "HospitalAdmin,Doctor,Receptionist")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(
        IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    // =========================================================
    // CREATE APPOINTMENT
    // =========================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateAppointmentRequest request)
    {
        var response =
            await _appointmentService.CreateAsync(request);

        return Ok(response);
    }

    // =========================================================
    // GET ALL APPOINTMENTS
    // =========================================================

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] AppointmentFilterRequest request)
    {
        var response =
            await _appointmentService.GetAllAsync(request);

        return Ok(response);
    }

    // =========================================================
    // GET APPOINTMENT BY ID
    // =========================================================

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id)
    {
        var response =
            await _appointmentService.GetByIdAsync(id);

        if (response == null)
            return NotFound();

        return Ok(response);
    }

    // =========================================================
    // UPDATE APPOINTMENT
    // =========================================================

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateAppointmentRequest request)
    {
        var response =
            await _appointmentService.UpdateAsync(
                id,
                request);

        return Ok(response);
    }

    // =========================================================
    // DELETE APPOINTMENT
    // =========================================================

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id)
    {
        var response =
            await _appointmentService.DeleteAsync(id);

        return Ok(response);
    }

    // =========================================================
    // GET DOCTOR AVAILABLE SLOTS
    // =========================================================

    [HttpGet("doctor/{doctorId:guid}/available-slots")]
    public async Task<IActionResult> GetDoctorAvailableSlots(
        Guid doctorId,
        [FromQuery] DateOnly appointmentDate)
    {
        var response =
            await _appointmentService.GetDoctorAvailableSlotsAsync(
                doctorId,
                appointmentDate);

        return Ok(response);
    }

    // =========================================================
    // UPDATE APPOINTMENT STATUS
    // =========================================================

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(
        Guid id,
        [FromQuery] Guid appointmentStatusId,
        [FromQuery] string? remarks = null)
    {
        var response =
            await _appointmentService.UpdateStatusAsync(
                id,
                appointmentStatusId,
                remarks);

        return Ok(response);
    }

    // =========================================================
    // GET APPOINTMENT HISTORY
    // =========================================================

    [HttpGet("{id:guid}/history")]
    public async Task<IActionResult> GetHistory(
        Guid id)
    {
        var response =
            await _appointmentService.GetHistoryAsync(id);

        return Ok(response);
    }

    // =========================================================
    // ADD APPOINTMENT NOTE
    // =========================================================

    [HttpPost("{id:guid}/notes")]
    public async Task<IActionResult> AddNote(
        Guid id,
        [FromBody] string note)
    {
        var response =
            await _appointmentService.AddNoteAsync(
                id,
                note);

        return Ok(response);
    }

    // =========================================================
    // GET APPOINTMENT NOTES
    // =========================================================

    [HttpGet("{id:guid}/notes")]
    public async Task<IActionResult> GetNotes(
        Guid id)
    {
        var response =
            await _appointmentService.GetNotesAsync(id);

        return Ok(response);
    }

    // =========================================================
    // ADD APPOINTMENT ATTACHMENT
    // =========================================================

    [HttpPost("{id:guid}/attachments")]
    public async Task<IActionResult> AddAttachment(
        Guid id,
        [FromQuery] string fileName,
        [FromQuery] string fileUrl,
        [FromQuery] string? fileType = null)
    {
        var response =
            await _appointmentService.AddAttachmentAsync(
                id,
                fileName,
                fileUrl,
                fileType);

        return Ok(response);
    }

    // =========================================================
    // GET APPOINTMENT ATTACHMENTS
    // =========================================================

    [HttpGet("{id:guid}/attachments")]
    public async Task<IActionResult> GetAttachments(
        Guid id)
    {
        var response =
            await _appointmentService.GetAttachmentsAsync(id);

        return Ok(response);
    }
}