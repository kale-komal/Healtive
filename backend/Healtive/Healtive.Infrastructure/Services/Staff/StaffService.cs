using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Staff;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;

namespace Healtive.Infrastructure.Services.Staff;

public class StaffService : IStaffService
{
    private readonly IStaffRepository _staffRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly IPasswordHasher _passwordHasher;

    public StaffService(
        IStaffRepository staffRepository,
        ICurrentUserService currentUser,
        IPasswordHasher passwordHasher)
    {
        _staffRepository = staffRepository;
        _currentUser = currentUser;
        _passwordHasher = passwordHasher;
    }

    public async Task<ApiResponse<StaffResponse>> CreateAsync(
        CreateStaffRequest request)
    {
        var hospitalId = _currentUser.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<StaffResponse>.FailureResponse(
                "Hospital context not found.");
        }

        if (!await _staffRepository.RoleExistsAsync(
                hospitalId,
                request.RoleId))
        {
            return ApiResponse<StaffResponse>.FailureResponse(
                "Invalid role.");
        }

        if (await _staffRepository.UsernameExistsAsync(
                hospitalId,
                request.Username))
        {
            return ApiResponse<StaffResponse>.FailureResponse(
                "Username already exists.");
        }

        if (await _staffRepository.EmployeeCodeExistsAsync(
                hospitalId,
                request.EmployeeCode))
        {
            return ApiResponse<StaffResponse>.FailureResponse(
                "Employee code already exists.");
        }

        if (await _staffRepository.EmailExistsAsync(
                hospitalId,
                request.Email))
        {
            return ApiResponse<StaffResponse>.FailureResponse(
                "Email already exists.");
        }

        if (await _staffRepository.MobileNumberExistsAsync(
                hospitalId,
                request.MobileNumber))
        {
            return ApiResponse<StaffResponse>.FailureResponse(
                "Mobile number already exists.");
        }

        var temporaryPassword =
            "Staff@" + Random.Shared.Next(1000, 9999);

        var user = new User
        {
            Id = Guid.NewGuid(),

            HospitalId = hospitalId,

            EmployeeCode = request.EmployeeCode,
            Username = request.Username,

            FirstName = request.FirstName,
            LastName = request.LastName,

            Email = request.Email,
            MobileNumber = request.MobileNumber,

            PasswordHash =
                _passwordHasher.HashPassword(
                    temporaryPassword),

            IsEmailVerified = false,
            IsMobileVerified = false,

            IsActive = true,

            CreatedAt = DateTime.UtcNow,

            IsDeleted = false
        };

        await _staffRepository.CreateAsync(user);

        await _staffRepository.AssignRoleAsync(
            new UserRole
            {
                UserId = user.Id,
                RoleId = request.RoleId,
                AssignedAt = DateTime.UtcNow
            });

        var response =
            await _staffRepository.GetByIdAsync(
                hospitalId,
                user.Id);

        if (response == null)
        {
            return ApiResponse<StaffResponse>.FailureResponse(
                "Staff created but details could not be loaded.");
        }

        return ApiResponse<StaffResponse>.SuccessResponse(
            response,
            $"Staff created successfully. Temporary password: {temporaryPassword}");
    }

    public async Task<ApiResponse<PagedResponse<StaffListResponse>>> GetAllAsync(
        StaffFilterRequest request)
    {
        var hospitalId = _currentUser.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<PagedResponse<StaffListResponse>>
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
            await _staffRepository.GetAllAsync(
                request,
                hospitalId);

        return ApiResponse<PagedResponse<StaffListResponse>>
            .SuccessResponse(
                result,
                "Staff fetched successfully.");
    }

    public async Task<ApiResponse<StaffResponse>> GetByIdAsync(
        Guid id)
    {
        var hospitalId = _currentUser.HospitalId;

        var staff =
            await _staffRepository.GetByIdAsync(
                hospitalId,
                id);

        if (staff == null)
        {
            return ApiResponse<StaffResponse>.FailureResponse(
                "Staff member not found.");
        }

        return ApiResponse<StaffResponse>.SuccessResponse(
            staff,
            "Staff fetched successfully.");
    }

    public async Task<ApiResponse<string>> UpdateAsync(
        Guid id,
        UpdateStaffRequest request)
    {
        var hospitalId = _currentUser.HospitalId;

        var user =
            await _staffRepository.GetUserByIdAsync(
                hospitalId,
                id);

        if (user == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Staff member not found.");
        }

        if (!await _staffRepository.RoleExistsAsync(
                hospitalId,
                request.RoleId))
        {
            return ApiResponse<string>.FailureResponse(
                "Invalid role.");
        }

        if (await _staffRepository.EmployeeCodeExistsAsync(
                hospitalId,
                id,
                request.EmployeeCode))
        {
            return ApiResponse<string>.FailureResponse(
                "Employee code already exists.");
        }

        if (await _staffRepository.EmailExistsAsync(
                hospitalId,
                id,
                request.Email))
        {
            return ApiResponse<string>.FailureResponse(
                "Email already exists.");
        }

        if (await _staffRepository.MobileNumberExistsAsync(
                hospitalId,
                id,
                request.MobileNumber))
        {
            return ApiResponse<string>.FailureResponse(
                "Mobile number already exists.");
        }

        user.EmployeeCode = request.EmployeeCode;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        user.MobileNumber = request.MobileNumber;
        user.UpdatedAt = DateTime.UtcNow;

        await _staffRepository.UpdateAsync(user);

        await _staffRepository.UpdateRoleAsync(
            id,
            request.RoleId);

        return ApiResponse<string>.SuccessResponse(
            "Staff updated successfully.",
            "Success");
    }

    public async Task<ApiResponse<string>> DeleteAsync(
        Guid id)
    {
        var hospitalId = _currentUser.HospitalId;

        var user =
            await _staffRepository.GetUserByIdAsync(
                hospitalId,
                id);

        if (user == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Staff member not found.");
        }

        await _staffRepository.DeleteAsync(
            hospitalId,
            id);

        return ApiResponse<string>.SuccessResponse(
            "Staff deleted successfully.",
            "Success");
    }

    public async Task<ApiResponse<string>> ActivateAsync(
        Guid id)
    {
        var hospitalId = _currentUser.HospitalId;

        var user =
            await _staffRepository.GetUserByIdAsync(
                hospitalId,
                id);

        if (user == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Staff member not found.");
        }

        if (user.IsActive)
        {
            return ApiResponse<string>.FailureResponse(
                "Staff member is already active.");
        }

        await _staffRepository.ActivateAsync(
            hospitalId,
            id);

        return ApiResponse<string>.SuccessResponse(
            "Staff activated successfully.",
            "Success");
    }

    public async Task<ApiResponse<string>> DeactivateAsync(
        Guid id)
    {
        var hospitalId = _currentUser.HospitalId;

        var user =
            await _staffRepository.GetUserByIdAsync(
                hospitalId,
                id);

        if (user == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Staff member not found.");
        }

        if (!user.IsActive)
        {
            return ApiResponse<string>.FailureResponse(
                "Staff member is already inactive.");
        }

        await _staffRepository.DeactivateAsync(
            hospitalId,
            id);

        return ApiResponse<string>.SuccessResponse(
            "Staff deactivated successfully.",
            "Success");
    }
}