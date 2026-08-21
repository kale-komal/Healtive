using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Role;

namespace Healtive.Application.Interfaces;

public interface IRoleService
{
    Task<ApiResponse<RoleResponse>> CreateAsync(
        CreateRoleRequest request);

    Task<ApiResponse<PagedResponse<RoleListResponse>>> GetAllAsync(
        RoleFilterRequest request);

    Task<ApiResponse<RoleResponse>> GetByIdAsync(
        Guid id);

    Task<ApiResponse<string>> UpdateAsync(
        Guid id,
        UpdateRoleRequest request);

    Task<ApiResponse<string>> DeleteAsync(
        Guid id);

    Task<ApiResponse<string>> ActivateAsync(
        Guid id);

    Task<ApiResponse<string>> DeactivateAsync(
        Guid id);
}