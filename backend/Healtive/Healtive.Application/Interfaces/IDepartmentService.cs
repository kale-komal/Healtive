using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Department;

namespace Healtive.Application.Interfaces;

public interface IDepartmentService
{
    Task<ApiResponse<string>> CreateAsync(
        CreateDepartmentRequest request);

    Task<ApiResponse<PagedResponse<DepartmentListResponse>>> GetAllAsync(
        string? search,
        bool? status,
        int page,
        int pageSize);

    Task<ApiResponse<DepartmentResponse>> GetByIdAsync(
        Guid id);

    Task<ApiResponse<string>> UpdateAsync(
        Guid id,
        UpdateDepartmentRequest request);

    Task<ApiResponse<string>> DeleteAsync(
        Guid id);

    Task<ApiResponse<string>> ActivateAsync(
        Guid id);

    Task<ApiResponse<string>> DeactivateAsync(
        Guid id);
}