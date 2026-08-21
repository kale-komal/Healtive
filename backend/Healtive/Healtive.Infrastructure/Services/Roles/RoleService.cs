using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Role;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;

namespace Healtive.Infrastructure.Services.Roles;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly ICurrentUserService _currentUser;

    public RoleService(
        IRoleRepository roleRepository,
        ICurrentUserService currentUser)
    {
        _roleRepository = roleRepository;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<RoleResponse>> CreateAsync(
        CreateRoleRequest request)
    {
        var hospitalId = _currentUser.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<RoleResponse>.FailureResponse(
                "Hospital context not found.");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return ApiResponse<RoleResponse>.FailureResponse(
                "Role name is required.");
        }

        var roleName = request.Name.Trim();

        if (await _roleRepository.ExistsByNameAsync(
                hospitalId,
                roleName))
        {
            return ApiResponse<RoleResponse>.FailureResponse(
                "Role already exists.");
        }

        var role = new Role
        {
            Id = Guid.NewGuid(),
            HospitalId = hospitalId,
            Name = roleName,
            Description = request.Description?.Trim(),

            // HospitalAdmin-created roles are custom roles.
            IsSystemRole = false,

            IsActive = true,

            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        await _roleRepository.CreateAsync(role);

        var response =
            await _roleRepository.GetByIdAsync(
                hospitalId,
                role.Id);

        if (response == null)
        {
            return ApiResponse<RoleResponse>.FailureResponse(
                "Role created but details could not be loaded.");
        }

        return ApiResponse<RoleResponse>.SuccessResponse(
            response,
            "Role created successfully.");
    }

    public async Task<ApiResponse<PagedResponse<RoleListResponse>>> GetAllAsync(
        RoleFilterRequest request)
    {
        var hospitalId = _currentUser.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<PagedResponse<RoleListResponse>>
                .FailureResponse(
                    "Hospital context not found.");
        }

        if (request.Page < 1)
            request.Page = 1;

        if (request.PageSize < 1)
            request.PageSize = 10;

        if (request.PageSize > 100)
            request.PageSize = 100;

        var result =
            await _roleRepository.GetAllAsync(
                hospitalId,
                request);

        return ApiResponse<PagedResponse<RoleListResponse>>
            .SuccessResponse(
                result,
                "Roles fetched successfully.");
    }

    public async Task<ApiResponse<RoleResponse>> GetByIdAsync(
        Guid id)
    {
        var hospitalId = _currentUser.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<RoleResponse>.FailureResponse(
                "Hospital context not found.");
        }

        var role =
            await _roleRepository.GetByIdAsync(
                hospitalId,
                id);

        if (role == null)
        {
            return ApiResponse<RoleResponse>.FailureResponse(
                "Role not found.");
        }

        return ApiResponse<RoleResponse>.SuccessResponse(
            role,
            "Role fetched successfully.");
    }

    public async Task<ApiResponse<string>> UpdateAsync(
        Guid id,
        UpdateRoleRequest request)
    {
        var hospitalId = _currentUser.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<string>.FailureResponse(
                "Hospital context not found.");
        }

        var role =
            await _roleRepository.GetEntityByIdAsync(
                hospitalId,
                id);

        if (role == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Role not found.");
        }

        // System roles cannot be modified.
        if (role.IsSystemRole)
        {
            return ApiResponse<string>.FailureResponse(
                "System roles cannot be modified.");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return ApiResponse<string>.FailureResponse(
                "Role name is required.");
        }

        var roleName = request.Name.Trim();

        if (await _roleRepository.ExistsByNameAsync(
                hospitalId,
                id,
                roleName))
        {
            return ApiResponse<string>.FailureResponse(
                "Role already exists.");
        }

        role.Name = roleName;
        role.Description = request.Description?.Trim();
        role.UpdatedAt = DateTime.UtcNow;

        await _roleRepository.UpdateAsync(role);

        return ApiResponse<string>.SuccessResponse(
            "Role updated successfully.",
            "Success");
    }

    public async Task<ApiResponse<string>> DeleteAsync(
        Guid id)
    {
        var hospitalId = _currentUser.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<string>.FailureResponse(
                "Hospital context not found.");
        }

        var role =
            await _roleRepository.GetEntityByIdAsync(
                hospitalId,
                id);

        if (role == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Role not found.");
        }

        // Never delete system roles.
        if (role.IsSystemRole)
        {
            return ApiResponse<string>.FailureResponse(
                "System roles cannot be deleted.");
        }

        await _roleRepository.DeleteAsync(
            hospitalId,
            id);

        return ApiResponse<string>.SuccessResponse(
            "Role deleted successfully.",
            "Success");
    }

    public async Task<ApiResponse<string>> ActivateAsync(
        Guid id)
    {
        var hospitalId = _currentUser.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<string>.FailureResponse(
                "Hospital context not found.");
        }

        var role =
            await _roleRepository.GetEntityByIdAsync(
                hospitalId,
                id);

        if (role == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Role not found.");
        }

        if (role.IsActive)
        {
            return ApiResponse<string>.FailureResponse(
                "Role is already active.");
        }

        await _roleRepository.ActivateAsync(
            hospitalId,
            id);

        return ApiResponse<string>.SuccessResponse(
            "Role activated successfully.",
            "Success");
    }

    public async Task<ApiResponse<string>> DeactivateAsync(
        Guid id)
    {
        var hospitalId = _currentUser.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<string>.FailureResponse(
                "Hospital context not found.");
        }

        var role =
            await _roleRepository.GetEntityByIdAsync(
                hospitalId,
                id);

        if (role == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Role not found.");
        }

        // Never deactivate system roles.
        if (role.IsSystemRole)
        {
            return ApiResponse<string>.FailureResponse(
                "System roles cannot be deactivated.");
        }

        if (!role.IsActive)
        {
            return ApiResponse<string>.FailureResponse(
                "Role is already inactive.");
        }

        await _roleRepository.DeactivateAsync(
            hospitalId,
            id);

        return ApiResponse<string>.SuccessResponse(
            "Role deactivated successfully.",
            "Success");
    }
}