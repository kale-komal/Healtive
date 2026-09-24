using Healtive.Application.DTOs.Appointment;
using Healtive.Application.DTOs.Common;
using Healtive.Core.Entities;

namespace Healtive.Application.Interfaces;

public interface IAppointmentRepository
{
    Task<bool> ExistsAsync(
        Guid hospitalId,
        Guid appointmentId);

    Task<bool> HasConflictAsync(
        Guid doctorId,
        DateOnly appointmentDate,
        TimeSpan appointmentTime,
        Guid? excludeAppointmentId = null);

    Task CreateAsync(
        Appointment appointment);

    Task UpdateAsync(
        Appointment appointment);

    Task<AppointmentResponse?> GetByIdAsync(
        Guid hospitalId,
        Guid appointmentId);

    Task<PagedResponse<AppointmentResponse>> GetAllAsync(
        Guid hospitalId,
        AppointmentFilterRequest request);

    Task<IEnumerable<DoctorAvailableSlotResponse>>
        GetDoctorAvailableSlotsAsync(
            Guid hospitalId,
            Guid doctorId,
            DateOnly appointmentDate);

    Task DeleteAsync(
        Guid hospitalId,
        Guid appointmentId);

    Task UpdateStatusAsync(
        Guid hospitalId,
        Guid appointmentId,
        Guid appointmentStatusId);

    Task<AppointmentCheckInResult> CheckInAsync(
        Guid hospitalId,
        Guid appointmentId,
        Guid? doctorId,
        Guid changedByUserId,
        string remark);

    Task<IEnumerable<AppointmentQueueItemResponse>> GetQueueAsync(
        Guid hospitalId,
        Guid doctorId,
        DateOnly appointmentDate);

    Task<string?> GetUserNameAsync(
        Guid hospitalId,
        Guid userId);

    Task AddHistoryAsync(
        AppointmentHistory history);

    Task<IEnumerable<AppointmentHistory>>
        GetHistoryAsync(
            Guid hospitalId,
            Guid appointmentId);

    Task AddNoteAsync(
        AppointmentNote note);

    Task AddAttachmentAsync(
        AppointmentAttachment attachment);

    Task<IEnumerable<AppointmentNote>>
        GetNotesAsync(
            Guid hospitalId,
            Guid appointmentId);

    Task<IEnumerable<AppointmentAttachment>>
        GetAttachmentsAsync(
            Guid hospitalId,
            Guid appointmentId);
}