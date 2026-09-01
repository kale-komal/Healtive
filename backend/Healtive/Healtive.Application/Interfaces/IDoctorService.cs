using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Doctor;

namespace Healtive.Application.Interfaces;

public interface IDoctorService
{
    Task<ApiResponse<DoctorResponse>> CreateAsync(
        CreateDoctorRequest request);

    Task<ApiResponse<PagedResponse<DoctorListResponse>>> GetAllAsync(
        DoctorFilterRequest request);

    Task<ApiResponse<DoctorResponse>> GetByIdAsync(
        Guid id);

    Task<ApiResponse<string>> UpdateAsync(
        Guid id,
        UpdateDoctorRequest request);

    Task<ApiResponse<string>> DeleteAsync(
        Guid id);

    Task<ApiResponse<string>> ActivateAsync(
        Guid id);

    Task<ApiResponse<string>> DeactivateAsync(
        Guid id);
    Task<ApiResponse<string>>
    ResetPasswordAsync(Guid doctorId);
}