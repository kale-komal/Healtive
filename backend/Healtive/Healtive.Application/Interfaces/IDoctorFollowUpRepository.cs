using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Doctor.FollowUp;

namespace Healtive.Application.Interfaces.Repositories;

public interface IDoctorFollowUpRepository
{
    Task<IEnumerable<FollowUpResponse>> GetUpcomingAsync(
        Guid hospitalId,
        Guid doctorId);

    Task<IEnumerable<FollowUpResponse>> GetByAppointmentIdAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid appointmentId);

    Task<FollowUpResponse?> GetByIdAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid followUpId);

    Task<ApiResponse<FollowUpResponse>> CreateAsync(
        Guid hospitalId,
        Guid doctorId,
        CreateFollowUpRequest request);

    Task<ApiResponse<FollowUpResponse>> UpdateAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid followUpId,
        UpdateFollowUpRequest request);

    Task<ApiResponse<FollowUpResponse>> CompleteAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid followUpId);

    Task<ApiResponse<FollowUpResponse>> CancelAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid followUpId);
}