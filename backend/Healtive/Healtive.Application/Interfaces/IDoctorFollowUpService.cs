using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Doctor.FollowUp;

namespace Healtive.Application.Interfaces;

public interface IDoctorFollowUpService
{
    Task<ApiResponse<IEnumerable<FollowUpResponse>>> GetUpcomingAsync();

    Task<ApiResponse<IEnumerable<FollowUpResponse>>> GetByAppointmentIdAsync(
        Guid appointmentId);

    Task<ApiResponse<FollowUpResponse>> GetByIdAsync(
        Guid followUpId);

    Task<ApiResponse<FollowUpResponse>> CreateAsync(
        CreateFollowUpRequest request);

    Task<ApiResponse<FollowUpResponse>> UpdateAsync(
        Guid followUpId,
        UpdateFollowUpRequest request);

    Task<ApiResponse<FollowUpResponse>> CompleteAsync(
        Guid followUpId);

    Task<ApiResponse<FollowUpResponse>> CancelAsync(
        Guid followUpId);
}