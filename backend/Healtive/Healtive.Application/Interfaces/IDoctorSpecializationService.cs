using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.DoctorSpecialization;

namespace Healtive.Application.Interfaces;

public interface IDoctorSpecializationService
{
    Task<ApiResponse<DoctorSpecializationResponse>> CreateAsync(
        CreateDoctorSpecializationRequest request);

    Task<ApiResponse<PagedResponse<DoctorSpecializationResponse>>> GetAllAsync(
        DoctorSpecializationFilterRequest request);

    Task<ApiResponse<DoctorSpecializationResponse>> GetByIdAsync(
        Guid id);

    Task<ApiResponse<string>> UpdateAsync(
        Guid id,
        UpdateDoctorSpecializationRequest request);

    Task<ApiResponse<string>> DeleteAsync(
        Guid id);

    Task<ApiResponse<string>> ActivateAsync(
        Guid id);

    Task<ApiResponse<string>> DeactivateAsync(
        Guid id);
}