using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Department;
using Healtive.Core.Entities;

namespace Healtive.Application.Interfaces;

public interface IDepartmentRepository
{
    Task CreateAsync(Department department);

    Task<PagedResponse<DepartmentListResponse>> GetAllAsync(
        Guid hospitalId,
        string? search,
        bool? status,
        int page,
        int pageSize);

    Task<Department?> GetByIdAsync(
        Guid hospitalId,
        Guid departmentId);

    Task<bool> ExistsByCodeAsync(
        Guid hospitalId,
        string code);

    Task<bool> ExistsByCodeAsync(
        Guid hospitalId,
        Guid departmentId,
        string code);

    Task UpdateAsync(Department department);

    Task DeleteAsync(
        Guid hospitalId,
        Guid departmentId);

    Task ActivateAsync(
        Guid hospitalId,
        Guid departmentId);

    Task DeactivateAsync(
        Guid hospitalId,
        Guid departmentId);
}