using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Hospital;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;

namespace Healtive.Infrastructure.Services.Hospitals;

public class HospitalService : IHospitalService
{
    private readonly IHospitalRepository _hospitalRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly IPasswordHasher _passwordHasher;

    public HospitalService(
        IHospitalRepository hospitalRepository,
        ICurrentUserService currentUser,
        IPasswordHasher passwordHasher)
    {
        _hospitalRepository = hospitalRepository;
        _currentUser = currentUser;
        _passwordHasher = passwordHasher;
    }

    public async Task<ApiResponse<HospitalResponse>> CreateAsync(
    CreateHospitalRequest request)
    {
        var lastCode = await _hospitalRepository.GetLastHospitalCodeAsync();

        string hospitalCode;

        if (string.IsNullOrEmpty(lastCode))
        {
            hospitalCode = "HSP000001";
        }
        else
        {
            var number = int.Parse(lastCode.Replace("HSP", ""));

            hospitalCode = $"HSP{(number + 1):D6}";
        }

        //if (await _hospitalRepository.ExistsByCodeAsync(request.Code))
        //{
        //    return ApiResponse<HospitalResponse>.FailureResponse(
        //        "Hospital code already exists.");
        //}

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
            Code = hospitalCode,

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
        Console.WriteLine("Hospital Saved");

        // Generate Hospital Admin Login
        var username = request.PhoneNumber;

        // Default password (Development)
        var temporaryPassword = "Hosp@123";

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
        Console.WriteLine("Role Saved");
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
        Console.WriteLine("User Saved");
        // Assign Role
        await _hospitalRepository.AssignRoleAsync(new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id,
            AssignedAt = DateTime.UtcNow
        });
        Console.WriteLine("Role Assigned");
        var response = new HospitalResponse
        {
            HospitalId = hospital.Id,

            Name = hospital.Name,
            Code = hospital.Code,

            LicenseNumber = hospital.LicenseNumber,
            GSTNumber = hospital.GSTNumber,

            HospitalType = hospital.HospitalType,

            Email = hospital.Email,
            PhoneNumber = hospital.PhoneNumber,
            Website = hospital.Website,

            Address = hospital.Address,
            City = hospital.City,
            State = hospital.State,
            Country = hospital.Country,
            PostalCode = hospital.PostalCode,

            TimeZone = hospital.TimeZone,
            Currency = hospital.Currency,

            IsActive = hospital.IsActive,

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

        var response = ToHospitalResponse(hospital);

        return ApiResponse<HospitalResponse>.SuccessResponse(
            response,
            "Hospital fetched successfully.");
    }

    public async Task<ApiResponse<HospitalResponse>> GetCurrentAsync()
    {
        var hospitalId = _currentUser.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<HospitalResponse>.FailureResponse(
                "Hospital context not found.");
        }

        var hospital =
            await _hospitalRepository.GetByIdAsync(hospitalId);

        if (hospital == null)
        {
            return ApiResponse<HospitalResponse>.FailureResponse(
                "Hospital not found.");
        }

        var response = ToHospitalResponse(hospital);

        return ApiResponse<HospitalResponse>.SuccessResponse(
            response,
            "Hospital fetched successfully.");
    }

    public async Task<ApiResponse<string>> UpdateCurrentAsync(
        UpdateHospitalRequest request)
    {
        var hospitalId = _currentUser.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<string>.FailureResponse(
                "Hospital context not found.");
        }

        var hospital =
            await _hospitalRepository.GetByIdAsync(hospitalId);

        if (hospital == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Hospital not found.");
        }

        if (await _hospitalRepository.ExistsByEmailAsync(
                hospitalId,
                request.Email))
        {
            return ApiResponse<string>.FailureResponse(
                "Email already exists.");
        }

        if (await _hospitalRepository.ExistsByMobileAsync(
                hospitalId,
                request.PhoneNumber))
        {
            return ApiResponse<string>.FailureResponse(
                "Phone number already exists.");
        }

        // Keep the HospitalAdmin login/contact record in sync with the
        // hospital contact information. The admin user is resolved from
        // the current hospital only; the request cannot target it.
        var adminUser =
            await _hospitalRepository.GetHospitalAdminUserAsync(
                hospitalId);

        if (adminUser != null)
        {
            var emailChanged =
                !string.Equals(
                    adminUser.Email,
                    request.Email,
                    StringComparison.OrdinalIgnoreCase);

            var mobileChanged =
                adminUser.MobileNumber != request.PhoneNumber;

            if (adminUser.Username == adminUser.MobileNumber)
            {
                // Onboarding uses the hospital phone number as the admin
                // username, so keep the login identifier in sync too.
                if (mobileChanged &&
                    await _hospitalRepository.UserUsernameExistsAsync(
                        hospitalId,
                        adminUser.Id,
                        request.PhoneNumber))
                {
                    return ApiResponse<string>.FailureResponse(
                        "Phone number is already used as a username.");
                }

                adminUser.Username = request.PhoneNumber;
            }

            if (emailChanged &&
                await _hospitalRepository.UserEmailExistsAsync(
                    hospitalId,
                    adminUser.Id,
                    request.Email))
            {
                return ApiResponse<string>.FailureResponse(
                    "Email is already used by another user.");
            }

            if (mobileChanged &&
                await _hospitalRepository.UserMobileExistsAsync(
                    hospitalId,
                    adminUser.Id,
                    request.PhoneNumber))
            {
                return ApiResponse<string>.FailureResponse(
                    "Phone number is already used by another user.");
            }

            adminUser.Email = request.Email;
            adminUser.MobileNumber = request.PhoneNumber;
            adminUser.UpdatedAt = DateTime.UtcNow;
        }

        hospital.Name = request.Name;
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

        if (adminUser != null)
        {
            await _hospitalRepository.UpdateHospitalAdminUserAsync(
                hospitalId,
                adminUser.Id,
                adminUser.Username,
                adminUser.Email,
                adminUser.MobileNumber);
        }

        return ApiResponse<string>.SuccessResponse(
            "Hospital updated successfully.",
            "Success");
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

    private static HospitalResponse ToHospitalResponse(
        Hospital hospital)
    {
        return new HospitalResponse
        {
            HospitalId = hospital.Id,
            Name = hospital.Name,
            Code = hospital.Code,
            LicenseNumber = hospital.LicenseNumber,
            GSTNumber = hospital.GSTNumber,
            HospitalType = hospital.HospitalType,
            Email = hospital.Email,
            PhoneNumber = hospital.PhoneNumber,
            Website = hospital.Website,
            Address = hospital.Address,
            City = hospital.City,
            State = hospital.State,
            Country = hospital.Country,
            PostalCode = hospital.PostalCode,
            TimeZone = hospital.TimeZone,
            Currency = hospital.Currency,
            IsActive = hospital.IsActive,
            CreatedAt = hospital.CreatedAt,
            AdminUsername = string.Empty,
            TemporaryPassword = string.Empty,
            PlanName = string.Empty
        };
    }
}