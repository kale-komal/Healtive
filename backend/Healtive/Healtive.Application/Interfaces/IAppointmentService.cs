using Healtive.Application.DTOs.Appointment;
using Healtive.Application.DTOs.Common;

namespace Healtive.Application.Interfaces;

public interface IAppointmentService
{
    Task<AppointmentResponse> CreateAsync(
        CreateAppointmentRequest request);

    Task<AppointmentResponse> UpdateAsync(
        Guid appointmentId,
        UpdateAppointmentRequest request);

    Task<AppointmentResponse?> GetByIdAsync(
        Guid appointmentId);

    Task<PagedResponse<AppointmentResponse>> GetAllAsync(
        AppointmentFilterRequest request);

    Task<IEnumerable<DoctorAvailableSlotResponse>>
        GetDoctorAvailableSlotsAsync(
            Guid doctorId,
            DateOnly appointmentDate);

    Task<bool> DeleteAsync(
        Guid appointmentId);

    Task<bool> UpdateStatusAsync(
        Guid appointmentId,
        Guid appointmentStatusId,
        string? remarks = null);

    Task<IEnumerable<AppointmentHistoryResponse>>
        GetHistoryAsync(
            Guid appointmentId);

    Task<bool> AddNoteAsync(
        Guid appointmentId,
        string note);

    Task<bool> AddAttachmentAsync(
        Guid appointmentId,
        string fileName,
        string fileUrl,
        string? fileType);

    Task<IEnumerable<AppointmentNoteResponse>>
        GetNotesAsync(
            Guid appointmentId);

    Task<IEnumerable<AppointmentAttachmentResponse>>
        GetAttachmentsAsync(
            Guid appointmentId);
}