using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Staff;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;
using MySqlConnector;

namespace Healtive.Infrastructure.Services.Staff;

public class StaffService : IStaffService
{
    private static readonly HashSet<string> NonStaffRoleNames =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "HospitalAdmin",
            "SuperAdmin",
            "Doctor"
        };

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

        var firstName = request.FirstName?.Trim() ?? string.Empty;
        var lastName = request.LastName?.Trim() ?? string.Empty;
        var email = request.Email?.Trim().ToLowerInvariant() ?? string.Empty;
        var mobileNumber = request.MobileNumber?.Trim() ?? string.Empty;
        var username = request.Username?.Trim() ?? string.Empty;

        if (firstName.Length == 0)
        {
            return ApiResponse<StaffResponse>.FailureResponse(
                "First name is required.");
        }

        if (lastName.Length == 0)
        {
            return ApiResponse<StaffResponse>.FailureResponse(
                "Last name is required.");
        }

        if (email.Length == 0)
        {
            return ApiResponse<StaffResponse>.FailureResponse(
                "Email is required.");
        }

        if (mobileNumber.Length == 0)
        {
            return ApiResponse<StaffResponse>.FailureResponse(
                "Mobile number is required.");
        }

        if (username.Length == 0)
        {
            return ApiResponse<StaffResponse>.FailureResponse(
                "Username is required.");
        }

        var roleName =
            await _staffRepository.GetAssignableRoleNameAsync(
                hospitalId,
                request.RoleId);

        if (roleName == null)
        {
            return ApiResponse<StaffResponse>.FailureResponse(
                "Invalid role. Doctor, HospitalAdmin and SuperAdmin roles cannot be assigned to staff.");
        }

        if (await _staffRepository.UsernameExistsAsync(
                hospitalId,
                username))
        {
            return ApiResponse<StaffResponse>.FailureResponse(
                "Username already exists.");
        }

        if (await _staffRepository.EmailExistsAsync(
                hospitalId,
                email))
        {
            return ApiResponse<StaffResponse>.FailureResponse(
                "Email already exists.");
        }

        if (await _staffRepository.MobileNumberExistsAsync(
                hospitalId,
                mobileNumber))
        {
            return ApiResponse<StaffResponse>.FailureResponse(
                "Mobile number already exists.");
        }

        var employeeCode =
            await EnsureUniqueEmployeeCodeAsync(
                hospitalId,
                request.EmployeeCode,
                Guid.Empty,
                firstName,
                lastName);

        var temporaryPassword =
            "Staff@123";

        var user = new User
        {
            Id = Guid.NewGuid(),

            HospitalId = hospitalId,

            EmployeeCode = employeeCode,
            Username = username,

            FirstName = firstName,
            LastName = lastName,

            Email = email,
            MobileNumber = mobileNumber,

            PasswordHash =
                _passwordHasher.HashPassword(
                    temporaryPassword),

            IsEmailVerified = false,
            IsMobileVerified = false,

            IsActive = true,

            CreatedAt = DateTime.UtcNow,

            IsDeleted = false
        };

        try
        {
            await _staffRepository.CreateAsync(user);

            await _staffRepository.AssignRoleAsync(
                new UserRole
                {
                    UserId = user.Id,
                    RoleId = request.RoleId,
                    AssignedAt = DateTime.UtcNow
                });
        }
        catch (Exception ex) when (IsDuplicateKeyException(ex))
        {
            return ApiResponse<StaffResponse>.FailureResponse(
                MapDuplicateKeyMessage(ex));
        }

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

        if (!await IsStaffUserAsync(hospitalId, id))
        {
            return ApiResponse<StaffResponse>.FailureResponse(
                "Staff member not found.");
        }

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

        if (!await IsStaffUserAsync(hospitalId, id))
        {
            return ApiResponse<string>.FailureResponse(
                "Staff member not found.");
        }

        var user =
            await _staffRepository.GetUserByIdAsync(
                hospitalId,
                id);

        if (user == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Staff member not found.");
        }

        var roleName =
            await _staffRepository.GetAssignableRoleNameAsync(
                hospitalId,
                request.RoleId);

        if (roleName == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Invalid role. Doctor, HospitalAdmin and SuperAdmin roles cannot be assigned to staff.");
        }

        var firstName = request.FirstName?.Trim() ?? string.Empty;
        var lastName = request.LastName?.Trim() ?? string.Empty;
        var email = request.Email?.Trim().ToLowerInvariant() ?? string.Empty;
        var mobileNumber = request.MobileNumber?.Trim() ?? string.Empty;

        if (firstName.Length == 0)
        {
            return ApiResponse<string>.FailureResponse(
                "First name is required.");
        }

        if (lastName.Length == 0)
        {
            return ApiResponse<string>.FailureResponse(
                "Last name is required.");
        }

        if (email.Length == 0)
        {
            return ApiResponse<string>.FailureResponse(
                "Email is required.");
        }

        if (mobileNumber.Length == 0)
        {
            return ApiResponse<string>.FailureResponse(
                "Mobile number is required.");
        }

        if (await _staffRepository.EmailExistsAsync(
                hospitalId,
                id,
                email))
        {
            return ApiResponse<string>.FailureResponse(
                "Email already exists.");
        }

        if (await _staffRepository.MobileNumberExistsAsync(
                hospitalId,
                id,
                mobileNumber))
        {
            return ApiResponse<string>.FailureResponse(
                "Mobile number already exists.");
        }

        var employeeCode =
            await EnsureUniqueEmployeeCodeAsync(
                hospitalId,
                request.EmployeeCode,
                id,
                firstName,
                lastName);

        user.EmployeeCode = employeeCode;
        user.FirstName = firstName;
        user.LastName = lastName;
        user.Email = email;
        user.MobileNumber = mobileNumber;
        user.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _staffRepository.UpdateAsync(user);

            await _staffRepository.UpdateRoleAsync(
                id,
                request.RoleId);
        }
        catch (Exception ex) when (IsDuplicateKeyException(ex))
        {
            return ApiResponse<string>.FailureResponse(
                MapDuplicateKeyMessage(ex));
        }

        return ApiResponse<string>.SuccessResponse(
            "Staff updated successfully.",
            "Success");
    }

    public async Task<ApiResponse<string>> DeleteAsync(
        Guid id)
    {
        var hospitalId = _currentUser.HospitalId;

        if (!await IsStaffUserAsync(hospitalId, id))
        {
            return ApiResponse<string>.FailureResponse(
                "Staff member not found.");
        }

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

        if (!await IsStaffUserAsync(hospitalId, id))
        {
            return ApiResponse<string>.FailureResponse(
                "Staff member not found.");
        }

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

        if (!await IsStaffUserAsync(hospitalId, id))
        {
            return ApiResponse<string>.FailureResponse(
                "Staff member not found.");
        }

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

    private async Task<string> EnsureUniqueEmployeeCodeAsync(
        Guid hospitalId,
        string? requestedCode,
        Guid userId,
        string firstName,
        string lastName)
    {
        var baseCode = string.IsNullOrWhiteSpace(requestedCode)
            ? GenerateEmployeeCode(firstName, lastName)
            : requestedCode.Trim().ToUpperInvariant();

        if (string.IsNullOrWhiteSpace(baseCode))
        {
            baseCode = GenerateEmployeeCode(firstName, lastName);
        }

        var candidate = baseCode;
        var suffix = 1;

        while (await _staffRepository.EmployeeCodeExistsAsync(
                   hospitalId,
                   userId,
                   candidate))
        {
            candidate = $"{baseCode}-{++suffix}";
        }

        return candidate;
    }

    private static string GenerateEmployeeCode(
        string firstName,
        string lastName)
    {
        var combined =
            $"{firstName} {lastName}";

        var cleaned = new string(
            combined
                .ToUpperInvariant()
                .Select(c => char.IsLetterOrDigit(c) ? c : ' ')
                .ToArray());

        var tokens = cleaned.Split(
            ' ',
            StringSplitOptions.RemoveEmptyEntries);

        if (tokens.Length == 0)
            return string.Empty;

        var code = string.Join("-", tokens);

        if (code.Length > 50)
            code = code[..50];

        return code.TrimEnd('-');
    }

    private async Task<bool> IsStaffUserAsync(
        Guid hospitalId,
        Guid userId)
    {
        var roles =
            await _staffRepository.GetUserRoleNamesAsync(
                hospitalId,
                userId);

        return !roles.Any(
            role => NonStaffRoleNames.Contains(role));
    }

    private static bool IsDuplicateKeyException(Exception ex)
    {
        for (var current = ex;
            current != null;
            current = current.InnerException)
        {
            if (current is MySqlException { Number: 1062 })
                return true;
        }

        return false;
    }

    private static string MapDuplicateKeyMessage(
        Exception ex)
    {
        var key = FindDuplicateKey(ex);

        if (string.IsNullOrEmpty(key))
        {
            return "The username, email or mobile number is already in use.";
        }

        if (key.Contains("Email", StringComparison.OrdinalIgnoreCase))
            return "Email already exists.";

        if (key.Contains("Mobile", StringComparison.OrdinalIgnoreCase))
            return "Mobile number already exists.";

        if (key.Contains("Username", StringComparison.OrdinalIgnoreCase))
            return "Username already exists.";

        if (key.Contains("Employeecode", StringComparison.OrdinalIgnoreCase))
            return "Employee code already exists.";

        return "The username, email or mobile number is already in use.";
    }

    private static string? FindDuplicateKey(
        Exception ex)
    {
        for (var current = ex;
            current != null;
            current = current.InnerException)
        {
            if (current is MySqlException { Number: 1062 } mySql)
            {
                const string marker = "for key '";

                var message = mySql.Message ?? string.Empty;

                var index = message.IndexOf(
                    marker,
                    StringComparison.OrdinalIgnoreCase);

                if (index < 0)
                    return null;

                var start = index + marker.Length;

                var end = message.IndexOf('\'', start);

                if (end <= start)
                    return null;

                return message[start..end];
            }
        }

        return null;
    }
}