using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Staff;

namespace Healtive.Application.Interfaces;

public interface IStaffService
{
    Task<ApiResponse<StaffResponse>> CreateAsync(
        CreateStaffRequest request);

    Task<ApiResponse<PagedResponse<StaffListResponse>>> GetAllAsync(
        StaffFilterRequest request);

    Task<ApiResponse<StaffResponse>> GetByIdAsync(
        Guid id);

    Task<ApiResponse<string>> UpdateAsync(
        Guid id,
        UpdateStaffRequest request);

    Task<ApiResponse<string>> DeleteAsync(
        Guid id);

    Task<ApiResponse<string>> ActivateAsync(
        Guid id);

    Task<ApiResponse<string>> DeactivateAsync(
        Guid id);
}