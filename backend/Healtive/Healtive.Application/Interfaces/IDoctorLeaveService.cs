using Healtive.Application.DTOs.DoctorLeave;
using Healtive.Application.DTOs.Common;

namespace Healtive.Application.Interfaces;

public interface IDoctorLeaveService
{
    Task<ApiResponse<DoctorLeaveResponse>> CreateAsync(
        Guid doctorId,
        CreateDoctorLeaveRequest request);

    Task<ApiResponse<DoctorLeaveResponse>> UpdateAsync(
        Guid doctorId,
        Guid id,
        UpdateDoctorLeaveRequest request);

    Task<ApiResponse<IEnumerable<DoctorLeaveResponse>>>
        GetByDoctorAsync(
            Guid doctorId);

    Task<ApiResponse<bool>> DeleteAsync(
        Guid doctorId,
        Guid id);

    Task<ApiResponse<bool>> ApproveAsync(
        Guid doctorId,
        Guid id);

    Task<ApiResponse<bool>> RejectAsync(
        Guid doctorId,
        Guid id);
}