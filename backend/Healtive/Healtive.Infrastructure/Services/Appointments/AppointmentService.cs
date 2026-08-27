using Healtive.Application.DTOs.Appointment;
using Healtive.Application.DTOs.Common;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Healtive.Infrastructure.Services.Appointments;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppointmentService(
        IAppointmentRepository appointmentRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _appointmentRepository = appointmentRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    // =========================================================
    // CREATE
    // =========================================================

    public async Task<AppointmentResponse> CreateAsync(
        CreateAppointmentRequest request)
    {
        var hospitalId = GetHospitalId();
        var userId = GetUserId();

        if (request.AppointmentDate < DateOnly.FromDateTime(DateTime.Today))
            throw new InvalidOperationException(
                "Appointment date cannot be in the past.");

        if (request.AppointmentDate == DateOnly.FromDateTime(DateTime.Today) &&
            request.AppointmentTime <= DateTime.Now.TimeOfDay)
        {
            throw new InvalidOperationException(
                "Appointment time must be in the future.");
        }

        var hasConflict =
            await _appointmentRepository.HasConflictAsync(
                request.DoctorId,
                request.AppointmentDate,
                request.AppointmentTime);

        if (hasConflict)
            throw new InvalidOperationException(
                "Doctor already has an appointment at this time.");

        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),

            AppointmentNumber = GenerateAppointmentNumber(),

            HospitalId = hospitalId,

            BranchId = request.BranchId,
            PatientId = request.PatientId,
            DoctorId = request.DoctorId,
            DepartmentId = request.DepartmentId,

            AppointmentStatusId = request.AppointmentStatusId,

            AppointmentDate = request.AppointmentDate,
            AppointmentTime = request.AppointmentTime,

            TokenNumber = request.TokenNumber,

            ConsultationType = request.ConsultationType,
            ReasonForVisit = request.ReasonForVisit,
            Notes = request.Notes,

            IsFirstVisit = request.IsFirstVisit,

            CreatedByUserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _appointmentRepository.CreateAsync(appointment);

        // Add initial history
        var history = new AppointmentHistory
        {
            Id = Guid.NewGuid(),

            AppointmentId = appointment.Id,

            AppointmentStatusId =
                appointment.AppointmentStatusId,

            ChangedByUserId = userId,

            Remarks = "Appointment created.",

            ChangedAt = DateTime.UtcNow
        };

        await _appointmentRepository.AddHistoryAsync(history);

        var response =
            await _appointmentRepository.GetByIdAsync(
                hospitalId,
                appointment.Id);

        if (response == null)
            throw new InvalidOperationException(
                "Appointment was created but could not be retrieved.");

        return response;
    }

    // =========================================================
    // UPDATE
    // =========================================================

    public async Task<AppointmentResponse> UpdateAsync(
        Guid appointmentId,
        UpdateAppointmentRequest request)
    {
        var hospitalId = GetHospitalId();

        var existing =
            await _appointmentRepository.GetByIdAsync(
                hospitalId,
                appointmentId);

        if (existing == null)
            throw new KeyNotFoundException(
                "Appointment not found.");

        if (request.AppointmentDate < DateOnly.FromDateTime(DateTime.Today))
            throw new InvalidOperationException(
                "Appointment date cannot be in the past.");

        var hasConflict =
            await _appointmentRepository.HasConflictAsync(
                request.DoctorId,
                request.AppointmentDate,
                request.AppointmentTime,
                appointmentId);

        if (hasConflict)
            throw new InvalidOperationException(
                "Doctor already has an appointment at this time.");

        var appointment = new Appointment
        {
            Id = appointmentId,

            HospitalId = hospitalId,

            BranchId = request.BranchId,
            PatientId = existing.PatientId,
            DoctorId = request.DoctorId,
            DepartmentId = request.DepartmentId,

            AppointmentStatusId =
                existing.AppointmentStatusId,

            AppointmentDate = request.AppointmentDate,
            AppointmentTime = request.AppointmentTime,

            TokenNumber = request.TokenNumber,

            ConsultationType = request.ConsultationType,
            ReasonForVisit = request.ReasonForVisit,
            Notes = request.Notes,

            IsFirstVisit = request.IsFirstVisit,

            UpdatedAt = DateTime.UtcNow
        };

        await _appointmentRepository.UpdateAsync(appointment);

        var response =
            await _appointmentRepository.GetByIdAsync(
                hospitalId,
                appointmentId);

        if (response == null)
            throw new InvalidOperationException(
                "Appointment could not be retrieved after update.");

        return response;
    }

    // =========================================================
    // GET BY ID
    // =========================================================

    public async Task<AppointmentResponse?> GetByIdAsync(
        Guid appointmentId)
    {
        var hospitalId = GetHospitalId();

        return await _appointmentRepository.GetByIdAsync(
            hospitalId,
            appointmentId);
    }

    // =========================================================
    // GET ALL
    // =========================================================

    public async Task<PagedResponse<AppointmentResponse>> GetAllAsync(
        AppointmentFilterRequest request)
    {
        var hospitalId = GetHospitalId();

        return await _appointmentRepository.GetAllAsync(
            hospitalId,
            request);
    }

    // =========================================================
    // DOCTOR AVAILABLE SLOTS
    // =========================================================

    public async Task<IEnumerable<DoctorAvailableSlotResponse>>
        GetDoctorAvailableSlotsAsync(
            Guid doctorId,
            DateOnly appointmentDate)
    {
        var hospitalId = GetHospitalId();

        if (appointmentDate < DateOnly.FromDateTime(DateTime.Today))
            throw new InvalidOperationException(
                "Cannot check availability for a past date.");

        return await _appointmentRepository
            .GetDoctorAvailableSlotsAsync(
                hospitalId,
                doctorId,
                appointmentDate);
    }

    // =========================================================
    // DELETE
    // =========================================================

    public async Task<bool> DeleteAsync(
        Guid appointmentId)
    {
        var hospitalId = GetHospitalId();

        var exists =
            await _appointmentRepository.ExistsAsync(
                hospitalId,
                appointmentId);

        if (!exists)
            throw new KeyNotFoundException(
                "Appointment not found.");

        await _appointmentRepository.DeleteAsync(
            hospitalId,
            appointmentId);

        return true;
    }

    // =========================================================
    // UPDATE STATUS
    // =========================================================

    public async Task<bool> UpdateStatusAsync(
        Guid appointmentId,
        Guid appointmentStatusId,
        string? remarks = null)
    {
        var hospitalId = GetHospitalId();
        var userId = GetUserId();

        var exists =
            await _appointmentRepository.ExistsAsync(
                hospitalId,
                appointmentId);

        if (!exists)
            throw new KeyNotFoundException(
                "Appointment not found.");

        await _appointmentRepository.UpdateStatusAsync(
            hospitalId,
            appointmentId,
            appointmentStatusId);

        var history = new AppointmentHistory
        {
            Id = Guid.NewGuid(),

            AppointmentId = appointmentId,

            AppointmentStatusId = appointmentStatusId,

            ChangedByUserId = userId,

            Remarks = remarks,

            ChangedAt = DateTime.UtcNow
        };

        await _appointmentRepository.AddHistoryAsync(history);

        return true;
    }

    // =========================================================
    // HISTORY
    // =========================================================

    public async Task<IEnumerable<AppointmentHistoryResponse>>
        GetHistoryAsync(
            Guid appointmentId)
    {
        var hospitalId = GetHospitalId();

        var exists =
            await _appointmentRepository.ExistsAsync(
                hospitalId,
                appointmentId);

        if (!exists)
            throw new KeyNotFoundException(
                "Appointment not found.");

        var history =
            await _appointmentRepository.GetHistoryAsync(
                hospitalId,
                appointmentId);

        return history.Select(x => new AppointmentHistoryResponse
        {
            Id = x.Id,
            AppointmentId = x.AppointmentId,
            AppointmentStatusId = x.AppointmentStatusId,
            ChangedByUserId = x.ChangedByUserId,
            Remarks = x.Remarks,
            ChangedAt = x.ChangedAt
        });
    }

    // =========================================================
    // ADD NOTE
    // =========================================================

    public async Task<bool> AddNoteAsync(
        Guid appointmentId,
        string note)
    {
        var hospitalId = GetHospitalId();
        var userId = GetUserId();

        if (string.IsNullOrWhiteSpace(note))
            throw new ArgumentException(
                "Note cannot be empty.");

        var exists =
            await _appointmentRepository.ExistsAsync(
                hospitalId,
                appointmentId);

        if (!exists)
            throw new KeyNotFoundException(
                "Appointment not found.");

        var appointmentNote = new AppointmentNote
        {
            Id = Guid.NewGuid(),

            AppointmentId = appointmentId,

            Note = note.Trim(),

            CreatedByUserId = userId,

            CreatedAt = DateTime.UtcNow
        };

        await _appointmentRepository.AddNoteAsync(
            appointmentNote);

        return true;
    }

    // =========================================================
    // ADD ATTACHMENT
    // =========================================================

    public async Task<bool> AddAttachmentAsync(
        Guid appointmentId,
        string fileName,
        string fileUrl,
        string? fileType)
    {
        var hospitalId = GetHospitalId();
        var userId = GetUserId();

        var exists =
            await _appointmentRepository.ExistsAsync(
                hospitalId,
                appointmentId);

        if (!exists)
            throw new KeyNotFoundException(
                "Appointment not found.");

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException(
                "File name is required.");

        if (string.IsNullOrWhiteSpace(fileUrl))
            throw new ArgumentException(
                "File URL is required.");

        var attachment = new AppointmentAttachment
        {
            Id = Guid.NewGuid(),

            AppointmentId = appointmentId,

            FileName = fileName,

            FileUrl = fileUrl,

            FileType = fileType,

            UploadedByUserId = userId,

            UploadedAt = DateTime.UtcNow
        };

        await _appointmentRepository.AddAttachmentAsync(
            attachment);

        return true;
    }

    // =========================================================
    // GET NOTES
    // =========================================================

    public async Task<IEnumerable<AppointmentNoteResponse>>
        GetNotesAsync(
            Guid appointmentId)
    {
        var hospitalId = GetHospitalId();

        var exists =
            await _appointmentRepository.ExistsAsync(
                hospitalId,
                appointmentId);

        if (!exists)
            throw new KeyNotFoundException(
                "Appointment not found.");

        var notes =
            await _appointmentRepository.GetNotesAsync(
                hospitalId,
                appointmentId);

        return notes.Select(x => new AppointmentNoteResponse
        {
            Id = x.Id,
            AppointmentId = x.AppointmentId,
            Note = x.Note,
            CreatedByUserId = x.CreatedByUserId,
            CreatedAt = x.CreatedAt
        });
    }

    // =========================================================
    // GET ATTACHMENTS
    // =========================================================

    public async Task<IEnumerable<AppointmentAttachmentResponse>>
        GetAttachmentsAsync(
            Guid appointmentId)
    {
        var hospitalId = GetHospitalId();

        var exists =
            await _appointmentRepository.ExistsAsync(
                hospitalId,
                appointmentId);

        if (!exists)
            throw new KeyNotFoundException(
                "Appointment not found.");

        var attachments =
            await _appointmentRepository.GetAttachmentsAsync(
                hospitalId,
                appointmentId);

        return attachments.Select(x =>
            new AppointmentAttachmentResponse
            {
                Id = x.Id,
                AppointmentId = x.AppointmentId,
                FileName = x.FileName,
                FileUrl = x.FileUrl,
                FileType = x.FileType,
                UploadedByUserId = x.UploadedByUserId,
                UploadedAt = x.UploadedAt
            });
    }

    // =========================================================
    // HELPERS
    // =========================================================

    private Guid GetHospitalId()
    {
        var claim =
            _httpContextAccessor.HttpContext?
                .User.FindFirst("HospitalId")?.Value;

        if (!Guid.TryParse(claim, out var hospitalId))
            throw new UnauthorizedAccessException(
                "Hospital information is missing from the token.");

        return hospitalId;
    }

    private Guid GetUserId()
    {
        var claim =
            _httpContextAccessor.HttpContext?
                .User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier)
                ?.Value;

        if (!Guid.TryParse(claim, out var userId))
            throw new UnauthorizedAccessException(
                "User information is missing from the token.");

        return userId;
    }

    private static string GenerateAppointmentNumber()
    {
        return $"APT-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
    }
}