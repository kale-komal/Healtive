using Healtive.Application.DTOs.Branch;
using Healtive.Core.Entities;
using Healtive.Application.DTOs.Common;
namespace Healtive.Application.Interfaces;

public interface IBranchRepository
{
    Task CreateAsync(Branch branch);

    Task<PagedResponse<BranchListResponse>> GetAllAsync(
    Guid hospitalId,
    string? search,
    bool? status,
    int page,
    int pageSize);

    Task<Branch?> GetByIdAsync(
        Guid hospitalId,
        Guid branchId);

    Task UpdateAsync(Branch branch);

    Task DeleteAsync(
        Guid hospitalId,
        Guid branchId);

    Task<bool> ExistsByCodeAsync(
        Guid hospitalId,
        string code);

    Task<bool> ExistsByCodeAsync(
        Guid hospitalId,
        Guid branchId,
        string code);

    Task ActivateAsync(
        Guid hospitalId,
        Guid branchId);

    Task DeactivateAsync(
        Guid hospitalId,
        Guid branchId);

    Task ClearHeadOfficeAsync(
        Guid hospitalId);
}