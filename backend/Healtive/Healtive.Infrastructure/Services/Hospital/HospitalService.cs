using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Hospital;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;

namespace Healtive.Infrastructure.Services.Hospitals;

public class HospitalService : IHospitalService
{
    private readonly IHospitalRepository _hospitalRepository;
    private readonly IPasswordHasher _passwordHasher;

    public HospitalService(
    IHospitalRepository hospitalRepository,
    IPasswordHasher passwordHasher)
    {
        _hospitalRepository = hospitalRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<ApiResponse<HospitalResponse>> CreateAsync(
    CreateHospitalRequest request)
    {
        if (await _hospitalRepository.ExistsByCodeAsync(request.Code))
        {
            return ApiResponse<HospitalResponse>.FailureResponse(
                "Hospital code already exists.");
        }

        if (await _hospitalRepository.ExistsByEmailAsync(request.Email))
        {
            return ApiResponse<HospitalResponse>.FailureResponse(
                "Email already exists.");
        }

        if (await _hospitalRepository.ExistsByMobileAsync(request.PhoneNumber))
        {
            return ApiResponse<HospitalResponse>.FailureResponse(
                "Mobile number already exists.");
        }

        var hospital = new Hospital
        {
            Id = Guid.NewGuid(),

            Name = request.Name,
            Code = request.Code,

            LicenseNumber = request.LicenseNumber,
            GSTNumber = request.GSTNumber,

            HospitalType = request.HospitalType,

            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Website = request.Website,

            Address = request.Address,
            City = request.City,
            State = request.State,
            Country = request.Country,
            PostalCode = request.PostalCode,

            TimeZone = "Asia/Kolkata",
            Currency = "INR",

            LogoUrl = null,

            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        // Save Hospital
        await _hospitalRepository.CreateAsync(hospital);

        // Generate Hospital Admin Login
        var username = request.PhoneNumber;

        var temporaryPassword =
            "Hosp@" + Random.Shared.Next(1000, 9999);

        var passwordHash =
            _passwordHasher.HashPassword(temporaryPassword);

        // Create Hospital Admin Role
        var role = new Role
        {
            Id = Guid.NewGuid(),
            HospitalId = hospital.Id,
            Name = "HospitalAdmin",
            Description = "Hospital Administrator",
            IsSystemRole = true,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        await _hospitalRepository.CreateRoleAsync(role);

        // Create Hospital Admin User
        var user = new User
        {
            Id = Guid.NewGuid(),

            HospitalId = hospital.Id,

            Username = username,

            FirstName = request.Name,
            LastName = "Admin",

            Email = request.Email,

            MobileNumber = request.PhoneNumber,

            PasswordHash = passwordHash,

            IsActive = true,

            IsEmailVerified = false,
            IsMobileVerified = false,

            CreatedAt = DateTime.UtcNow,

            IsDeleted = false
        };

        await _hospitalRepository.CreateUserAsync(user);

        // Assign Role
        await _hospitalRepository.AssignRoleAsync(new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id,
            AssignedAt = DateTime.UtcNow
        });
        var response = new HospitalResponse
        {
            HospitalId = hospital.Id,
            Code = hospital.Code,
            HospitalName = hospital.Name,

            AdminUsername = username,
            TemporaryPassword = temporaryPassword,

            PlanName = string.Empty
        };

        return ApiResponse<HospitalResponse>.SuccessResponse(
            response,
            "Hospital created successfully.");
    }

    public async Task<ApiResponse<string>> DeleteAsync(Guid id)
    {
        var hospital = await _hospitalRepository.GetByIdAsync(id);

        if (hospital == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Hospital not found.");
        }

        await _hospitalRepository.DeleteAsync(id);

        return ApiResponse<string>.SuccessResponse(
            "Hospital deleted successfully.",
            "Success");
    }

    public async Task<ApiResponse<PagedResponse<HospitalListResponse>>> GetAllAsync(
    HospitalFilterRequest request)
    {
        var hospitals = await _hospitalRepository.GetAllAsync(request);
        return ApiResponse<PagedResponse<HospitalListResponse>>
    .SuccessResponse(
        hospitals,
        "Hospitals fetched successfully.");
    }

    public async Task<ApiResponse<HospitalResponse>> GetByIdAsync(Guid id)
    {
        var hospital = await _hospitalRepository.GetByIdAsync(id);

        if (hospital == null)
        {
            return ApiResponse<HospitalResponse>.FailureResponse(
                "Hospital not found.");
        }

        var response = new HospitalResponse
        {
            HospitalId = hospital.Id,
            Code = hospital.Code,
            HospitalName = hospital.Name,

            AdminUsername = string.Empty,
            TemporaryPassword = string.Empty,
            PlanName = string.Empty
        };

        return ApiResponse<HospitalResponse>.SuccessResponse(
            response,
            "Hospital fetched successfully.");
    }

    public async Task<ApiResponse<string>> UpdateAsync(
    Guid id,
    UpdateHospitalRequest request)
    {
        var hospital = await _hospitalRepository.GetByIdAsync(id);

        if (hospital == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Hospital not found.");
        }

        if (await _hospitalRepository.ExistsByCodeAsync(id, request.Code))
        {
            return ApiResponse<string>.FailureResponse(
                "Hospital code already exists.");
        }

        if (await _hospitalRepository.ExistsByEmailAsync(id, request.Email))
        {
            return ApiResponse<string>.FailureResponse(
                "Email already exists.");
        }

        if (await _hospitalRepository.ExistsByMobileAsync(id, request.PhoneNumber))
        {
            return ApiResponse<string>.FailureResponse(
                "Phone number already exists.");
        }

        hospital.Name = request.Name;
        hospital.Code = request.Code;
        hospital.LicenseNumber = request.LicenseNumber;
        hospital.GSTNumber = request.GSTNumber;
        hospital.HospitalType = request.HospitalType;
        hospital.Email = request.Email;
        hospital.PhoneNumber = request.PhoneNumber;
        hospital.Website = request.Website;
        hospital.Address = request.Address;
        hospital.City = request.City;
        hospital.State = request.State;
        hospital.Country = request.Country;
        hospital.PostalCode = request.PostalCode;
        hospital.TimeZone = request.TimeZone;
        hospital.Currency = request.Currency;
        hospital.UpdatedAt = DateTime.UtcNow;

        await _hospitalRepository.UpdateAsync(hospital);

        return ApiResponse<string>.SuccessResponse(
            "Hospital updated successfully.",
            "Success");
    }
    public async Task<ApiResponse<string>> ActivateAsync(Guid id)
    {
        var hospital = await _hospitalRepository.GetByIdAsync(id);

        if (hospital == null)
        {
            return ApiResponse<string>.FailureResponse("Hospital not found.");
        }

        if (hospital.IsActive)
        {
            return ApiResponse<string>.FailureResponse(
                "Hospital is already active.");
        }

        await _hospitalRepository.ActivateAsync(id);

        return ApiResponse<string>.SuccessResponse(
            "Hospital activated successfully.",
            "Success");
    }
    public async Task<ApiResponse<string>> DeactivateAsync(Guid id)
    {
        var hospital = await _hospitalRepository.GetByIdAsync(id);

        if (hospital == null)
        {
            return ApiResponse<string>.FailureResponse("Hospital not found.");
        }

        if (!hospital.IsActive)
        {
            return ApiResponse<string>.FailureResponse(
                "Hospital is already inactive.");
        }

        await _hospitalRepository.DeactivateAsync(id);

        return ApiResponse<string>.SuccessResponse(
            "Hospital deactivated successfully.",
            "Success");
    }
}