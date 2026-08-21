using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Role;
using Healtive.Core.Entities;

namespace Healtive.Application.Interfaces;

public interface IRoleRepository
{
    Task<bool> ExistsByNameAsync(
        Guid hospitalId,
        string name);

    Task<bool> ExistsByNameAsync(
        Guid hospitalId,
        Guid roleId,
        string name);

    Task CreateAsync(Role role);

    Task UpdateAsync(Role role);

    Task<PagedResponse<RoleListResponse>> GetAllAsync(
        Guid hospitalId,
        RoleFilterRequest request);

    Task<RoleResponse?> GetByIdAsync(
        Guid hospitalId,
        Guid roleId);

    Task<Role?> GetEntityByIdAsync(
        Guid hospitalId,
        Guid roleId);

    Task DeleteAsync(
        Guid hospitalId,
        Guid roleId);

    Task ActivateAsync(
        Guid hospitalId,
        Guid roleId);

    Task DeactivateAsync(
        Guid hospitalId,
        Guid roleId);
}