using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Department;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;

namespace Healtive.Infrastructure.Services.Departments;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ICurrentUserService _currentUserService;

    public DepartmentService(
        IDepartmentRepository departmentRepository,
        ICurrentUserService currentUserService)
    {
        _departmentRepository = departmentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<ApiResponse<string>> CreateAsync(
        CreateDepartmentRequest request)
    {
        var hospitalId = _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<string>.FailureResponse(
                "Hospital context not found.");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return ApiResponse<string>.FailureResponse(
                "Department name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Code))
        {
            return ApiResponse<string>.FailureResponse(
                "Department code is required.");
        }

        var code = request.Code.Trim().ToUpper();

        if (await _departmentRepository.ExistsByCodeAsync(
                hospitalId,
                code))
        {
            return ApiResponse<string>.FailureResponse(
                "Department code already exists.");
        }

        var department = new Department
        {
            Id = Guid.NewGuid(),

            HospitalId = hospitalId,

            Name = request.Name.Trim(),

            Code = code,

            Description = string.IsNullOrWhiteSpace(request.Description)
                ? null
                : request.Description.Trim(),

            IsActive = true,

            CreatedAt = DateTime.UtcNow,

            IsDeleted = false
        };

        await _departmentRepository.CreateAsync(department);

        return ApiResponse<string>.SuccessResponse(
            "Department created successfully.",
            "Success");
    }

    public async Task<ApiResponse<PagedResponse<DepartmentListResponse>>> GetAllAsync(
        string? search,
        bool? status,
        int page,
        int pageSize)
    {
        var hospitalId = _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<PagedResponse<DepartmentListResponse>>
                .FailureResponse(
                    "Hospital context not found.");
        }

        if (page < 1)
            page = 1;

        if (pageSize < 1)
            pageSize = 10;

        if (pageSize > 100)
            pageSize = 100;

        var result = await _departmentRepository.GetAllAsync(
            hospitalId,
            search,
            status,
            page,
            pageSize);

        return ApiResponse<PagedResponse<DepartmentListResponse>>
            .SuccessResponse(
                result,
                "Departments fetched successfully.");
    }

    public async Task<ApiResponse<DepartmentResponse>> GetByIdAsync(
        Guid id)
    {
        var hospitalId = _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<DepartmentResponse>.FailureResponse(
                "Hospital context not found.");
        }

        var department = await _departmentRepository.GetByIdAsync(
            hospitalId,
            id);

        if (department == null)
        {
            return ApiResponse<DepartmentResponse>.FailureResponse(
                "Department not found.");
        }

        var response = new DepartmentResponse
        {
            DepartmentId = department.Id,

            Name = department.Name,

            Code = department.Code,

            Description = department.Description,

            IsActive = department.IsActive,

            CreatedAt = department.CreatedAt,

            UpdatedAt = department.UpdatedAt
        };

        return ApiResponse<DepartmentResponse>.SuccessResponse(
            response,
            "Department fetched successfully.");
    }

    public async Task<ApiResponse<string>> UpdateAsync(
        Guid id,
        UpdateDepartmentRequest request)
    {
        var hospitalId = _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<string>.FailureResponse(
                "Hospital context not found.");
        }

        var department = await _departmentRepository.GetByIdAsync(
            hospitalId,
            id);

        if (department == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Department not found.");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return ApiResponse<string>.FailureResponse(
                "Department name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Code))
        {
            return ApiResponse<string>.FailureResponse(
                "Department code is required.");
        }

        var code = request.Code.Trim().ToUpper();

        if (await _departmentRepository.ExistsByCodeAsync(
                hospitalId,
                id,
                code))
        {
            return ApiResponse<string>.FailureResponse(
                "Department code already exists.");
        }

        department.Name = request.Name.Trim();

        department.Code = code;

        department.Description =
            string.IsNullOrWhiteSpace(request.Description)
                ? null
                : request.Description.Trim();

        department.UpdatedAt = DateTime.UtcNow;

        await _departmentRepository.UpdateAsync(department);

        return ApiResponse<string>.SuccessResponse(
            "Department updated successfully.",
            "Success");
    }

    public async Task<ApiResponse<string>> DeleteAsync(
        Guid id)
    {
        var hospitalId = _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<string>.FailureResponse(
                "Hospital context not found.");
        }

        var department = await _departmentRepository.GetByIdAsync(
            hospitalId,
            id);

        if (department == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Department not found.");
        }

        await _departmentRepository.DeleteAsync(
            hospitalId,
            id);

        return ApiResponse<string>.SuccessResponse(
            "Department deleted successfully.",
            "Success");
    }

    public async Task<ApiResponse<string>> ActivateAsync(
        Guid id)
    {
        var hospitalId = _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<string>.FailureResponse(
                "Hospital context not found.");
        }

        var department = await _departmentRepository.GetByIdAsync(
            hospitalId,
            id);

        if (department == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Department not found.");
        }

        if (department.IsActive)
        {
            return ApiResponse<string>.FailureResponse(
                "Department is already active.");
        }

        await _departmentRepository.ActivateAsync(
            hospitalId,
            id);

        return ApiResponse<string>.SuccessResponse(
            "Department activated successfully.",
            "Success");
    }

    public async Task<ApiResponse<string>> DeactivateAsync(
        Guid id)
    {
        var hospitalId = _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<string>.FailureResponse(
                "Hospital context not found.");
        }

        var department = await _departmentRepository.GetByIdAsync(
            hospitalId,
            id);

        if (department == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Department not found.");
        }

        if (!department.IsActive)
        {
            return ApiResponse<string>.FailureResponse(
                "Department is already inactive.");
        }

        await _departmentRepository.DeactivateAsync(
            hospitalId,
            id);

        return ApiResponse<string>.SuccessResponse(
            "Department deactivated successfully.",
            "Success");
    }
}