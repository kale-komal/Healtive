using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.DoctorAvailability;

namespace Healtive.Application.Interfaces;

public interface IDoctorAvailabilityService
{
    Task<ApiResponse<DoctorAvailabilityResponse>> CreateAsync(
        Guid doctorId,
        CreateDoctorAvailabilityRequest request);

    Task<ApiResponse<DoctorAvailabilityResponse>> UpdateAsync(
        Guid doctorId,
        Guid id,
        UpdateDoctorAvailabilityRequest request);

    Task<ApiResponse<IEnumerable<DoctorAvailabilityResponse>>>
        GetByDoctorAsync(
            Guid doctorId);

    Task<ApiResponse<bool>> DeleteAsync(
        Guid doctorId,
        Guid id);

    Task<ApiResponse<bool>> ActivateAsync(
        Guid doctorId,
        Guid id);

    Task<ApiResponse<bool>> DeactivateAsync(
        Guid doctorId,
        Guid id);
}