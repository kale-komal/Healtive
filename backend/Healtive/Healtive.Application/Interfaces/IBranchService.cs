using Healtive.Application.DTOs.Branch;
using Healtive.Application.DTOs.Common;

namespace Healtive.Application.Interfaces;

public interface IBranchService
{
    Task<ApiResponse<string>> CreateAsync(
        CreateBranchRequest request);

    Task<ApiResponse<PagedResponse<BranchListResponse>>> GetAllAsync(
        string? search,
        bool? status,
        int page,
        int pageSize);

    Task<ApiResponse<BranchResponse>> GetByIdAsync(
        Guid branchId);

    Task<ApiResponse<string>> UpdateAsync(
        Guid branchId,
        UpdateBranchRequest request);

    Task<ApiResponse<string>> DeleteAsync(
        Guid branchId);

    Task<ApiResponse<string>> ActivateAsync(
        Guid branchId);

    Task<ApiResponse<string>> DeactivateAsync(
        Guid branchId);
}