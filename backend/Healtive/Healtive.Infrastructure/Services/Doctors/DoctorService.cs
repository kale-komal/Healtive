using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Doctor;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;

namespace Healtive.Infrastructure.Services.Doctors;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _repository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPasswordHasher _passwordHasher;

    public DoctorService(
        IDoctorRepository repository,
        ICurrentUserService currentUserService,
        IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _currentUserService = currentUserService;
        _passwordHasher = passwordHasher;
    }

    public async Task<ApiResponse<DoctorResponse>> CreateAsync(
        CreateDoctorRequest request)
    {
        var hospitalId = _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<DoctorResponse>
                .FailureResponse(
                    "Hospital context not found.");
        }

        if (string.IsNullOrWhiteSpace(request.FullName))
        {
            return ApiResponse<DoctorResponse>
                .FailureResponse(
                    "Doctor name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.DoctorCode))
        {
            return ApiResponse<DoctorResponse>
                .FailureResponse(
                    "Doctor code is required.");
        }

        if (string.IsNullOrWhiteSpace(request.RegistrationNumber))
        {
            return ApiResponse<DoctorResponse>
                .FailureResponse(
                    "Registration number is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Qualification))
        {
            return ApiResponse<DoctorResponse>
                .FailureResponse(
                    "Qualification is required.");
        }

        if (request.ExperienceYears < 0)
        {
            return ApiResponse<DoctorResponse>
                .FailureResponse(
                    "Experience years cannot be negative.");
        }

        if (request.ConsultationFee < 0)
        {
            return ApiResponse<DoctorResponse>
                .FailureResponse(
                    "Consultation fee cannot be negative.");
        }

        if (string.IsNullOrWhiteSpace(request.Gender))
        {
            return ApiResponse<DoctorResponse>
                .FailureResponse(
                    "Gender is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return ApiResponse<DoctorResponse>
                .FailureResponse(
                    "Email is required.");
        }

        if (string.IsNullOrWhiteSpace(request.MobileNumber))
        {
            return ApiResponse<DoctorResponse>
                .FailureResponse(
                    "Mobile number is required.");
        }

        var doctorCode =
            request.DoctorCode.Trim().ToUpperInvariant();

        var registrationNumber =
            request.RegistrationNumber.Trim();

        var email =
            request.Email.Trim().ToLowerInvariant();

        var mobileNumber =
            request.MobileNumber.Trim();

        if (await _repository.ExistsByCodeAsync(
                hospitalId,
                doctorCode))
        {
            return ApiResponse<DoctorResponse>
                .FailureResponse(
                    "Doctor code already exists.");
        }

        if (await _repository.ExistsByRegistrationNumberAsync(
                registrationNumber))
        {
            return ApiResponse<DoctorResponse>
                .FailureResponse(
                    "Registration number already exists.");
        }

        if (await _repository.ExistsByEmailAsync(email))
        {
            return ApiResponse<DoctorResponse>
                .FailureResponse(
                    "Email already exists.");
        }

        if (await _repository.ExistsByMobileAsync(mobileNumber))
        {
            return ApiResponse<DoctorResponse>
                .FailureResponse(
                    "Mobile number already exists.");
        }

        var doctorId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        /*
         * Doctor login username
         *
         * For now we use mobile number.
         * Later this can be changed to a separate
         * username or employee code if required.
         */
        var username = mobileNumber;

        var temporaryPassword = "Doc@123";

        var passwordHash =
            _passwordHasher.HashPassword(
                temporaryPassword);

        var fullName =
            request.FullName.Trim();

        var nameParts =
            fullName.Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries);

        var firstName =
            nameParts.Length > 0
                ? nameParts[0]
                : fullName;

        var lastName =
            nameParts.Length > 1
                ? string.Join(
                    " ",
                    nameParts.Skip(1))
                : string.Empty;

        var now = DateTime.UtcNow;

        var role = await _repository.GetRoleByNameAsync(
            hospitalId,
            "Doctor");

        if (role == null)
        {
            return ApiResponse<DoctorResponse>
                .FailureResponse(
                    "Doctor role not found. Please create the Doctor role first.");
        }

        var user = new User
        {
            Id = userId,
            HospitalId = hospitalId,

            Username = username,

            FirstName = firstName,
            LastName = lastName,

            Email = email,
            MobileNumber = mobileNumber,

            PasswordHash = passwordHash,

            IsEmailVerified = false,
            IsMobileVerified = false,

            IsActive = true,

            CreatedAt = now,
            IsDeleted = false
        };

        var doctor = new Doctor
        {
            Id = doctorId,

            HospitalId = hospitalId,
            UserId = userId,

            FullName = fullName,

            DoctorCode = doctorCode,
            RegistrationNumber = registrationNumber,

            Qualification =
                request.Qualification.Trim(),

            ExperienceYears =
                request.ExperienceYears,

            ConsultationFee =
                request.ConsultationFee,

            Gender =
                request.Gender.Trim(),

            DateOfBirth =
                request.DateOfBirth,

            JoiningDate =
                request.JoiningDate,

            Bio =
                string.IsNullOrWhiteSpace(request.Bio)
                    ? null
                    : request.Bio.Trim(),

            ProfileImageUrl =
                string.IsNullOrWhiteSpace(
                    request.ProfileImageUrl)
                    ? null
                    : request.ProfileImageUrl.Trim(),

            IsAvailable = true,
            IsActive = true,

            CreatedAt = now,
            IsDeleted = false
        };

        var userRole = new UserRole
        {
            UserId = userId,
            RoleId = role.Id,
            AssignedAt = now
        };

        await _repository.CreateAsync(
            doctor,
            user,
            role,
            userRole);

        var response =
            new DoctorResponse
            {
                DoctorId = doctor.Id,
                HospitalId = doctor.HospitalId,
                UserId = doctor.UserId,

                FullName = doctor.FullName,
                DoctorCode = doctor.DoctorCode,
                RegistrationNumber =
                    doctor.RegistrationNumber,

                Qualification =
                    doctor.Qualification,

                ExperienceYears =
                    doctor.ExperienceYears,

                ConsultationFee =
                    doctor.ConsultationFee,

                Gender = doctor.Gender,

                DateOfBirth =
                    doctor.DateOfBirth,

                JoiningDate =
                    doctor.JoiningDate,

                Bio = doctor.Bio,

                ProfileImageUrl =
                    doctor.ProfileImageUrl,

                IsAvailable =
                    doctor.IsAvailable,

                IsActive =
                    doctor.IsActive,

                CreatedAt =
                    doctor.CreatedAt,

                UpdatedAt =
                    doctor.UpdatedAt
            };

        return ApiResponse<DoctorResponse>
            .SuccessResponse(
                response,
                $"Doctor created successfully. Temporary password: {temporaryPassword}");
    }

    public async Task<ApiResponse<PagedResponse<DoctorListResponse>>>
        GetAllAsync(
            DoctorFilterRequest request)
    {
        var hospitalId =
            _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<PagedResponse<DoctorListResponse>>
                .FailureResponse(
                    "Hospital context not found.");
        }

        if (request.Page < 1)
        {
            request.Page = 1;
        }

        if (request.PageSize < 1)
        {
            request.PageSize = 10;
        }

        if (request.PageSize > 100)
        {
            request.PageSize = 100;
        }

        var result =
            await _repository.GetAllAsync(
                hospitalId,
                request);

        return ApiResponse<PagedResponse<DoctorListResponse>>
            .SuccessResponse(
                result,
                "Doctors fetched successfully.");
    }

    public async Task<ApiResponse<DoctorResponse>>
        GetByIdAsync(Guid id)
    {
        var hospitalId =
            _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<DoctorResponse>
                .FailureResponse(
                    "Hospital context not found.");
        }

        var doctor =
            await _repository.GetByIdAsync(
                hospitalId,
                id);

        if (doctor == null)
        {
            return ApiResponse<DoctorResponse>
                .FailureResponse(
                    "Doctor not found.");
        }

        return ApiResponse<DoctorResponse>
            .SuccessResponse(
                doctor,
                "Doctor fetched successfully.");
    }

    public async Task<ApiResponse<string>>
        UpdateAsync(
            Guid id,
            UpdateDoctorRequest request)
    {
        var hospitalId =
            _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Hospital context not found.");
        }

        var doctor =
            await _repository.GetEntityByIdAsync(
                hospitalId,
                id);

        if (doctor == null)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Doctor not found.");
        }

        if (string.IsNullOrWhiteSpace(request.FullName))
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Doctor name is required.");
        }

        if (string.IsNullOrWhiteSpace(
                request.RegistrationNumber))
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Registration number is required.");
        }

        if (string.IsNullOrWhiteSpace(
                request.Qualification))
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Qualification is required.");
        }

        var registrationNumber =
            request.RegistrationNumber.Trim();

        var email =
            request.Email.Trim().ToLowerInvariant();

        var mobileNumber =
            request.MobileNumber.Trim();

        if (await _repository
            .ExistsByRegistrationNumberAsync(
                id,
                registrationNumber))
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Registration number already exists.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Email is required.");
        }

        if (string.IsNullOrWhiteSpace(mobileNumber))
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Mobile number is required.");
        }

        var user =
            doctor.UserId.HasValue
                ? await _repository.GetUserByIdAsync(
                    doctor.UserId.Value)
                : null;

        if (user == null)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Doctor login user not found.");
        }

        doctor.FullName =
            request.FullName.Trim();

        doctor.RegistrationNumber =
            registrationNumber;

        doctor.Qualification =
            request.Qualification.Trim();

        doctor.ExperienceYears =
            request.ExperienceYears;

        doctor.ConsultationFee =
            request.ConsultationFee;

        doctor.Gender =
            request.Gender.Trim();

        doctor.DateOfBirth =
            request.DateOfBirth;

        doctor.JoiningDate =
            request.JoiningDate;

        doctor.Bio =
            string.IsNullOrWhiteSpace(request.Bio)
                ? null
                : request.Bio.Trim();

        doctor.ProfileImageUrl =
            string.IsNullOrWhiteSpace(
                request.ProfileImageUrl)
                ? null
                : request.ProfileImageUrl.Trim();

        doctor.UpdatedAt =
            DateTime.UtcNow;

        user.Email = email;
        user.MobileNumber = mobileNumber;

        await _repository.UpdateAsync(
            doctor,
            user);

        return ApiResponse<string>
            .SuccessResponse(
                "Doctor updated successfully.",
                "Success");
    }

    public async Task<ApiResponse<string>>
        DeleteAsync(Guid id)
    {
        var hospitalId =
            _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Hospital context not found.");
        }

        var doctor =
            await _repository.GetEntityByIdAsync(
                hospitalId,
                id);

        if (doctor == null)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Doctor not found.");
        }

        await _repository.DeleteAsync(
            hospitalId,
            id);

        return ApiResponse<string>
            .SuccessResponse(
                "Doctor deleted successfully.",
                "Success");
    }

    public async Task<ApiResponse<string>>
        ActivateAsync(Guid id)
    {
        var hospitalId =
            _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Hospital context not found.");
        }

        var doctor =
            await _repository.GetEntityByIdAsync(
                hospitalId,
                id);

        if (doctor == null)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Doctor not found.");
        }

        if (doctor.IsActive)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Doctor is already active.");
        }

        await _repository.ActivateAsync(
            hospitalId,
            id);

        return ApiResponse<string>
            .SuccessResponse(
                "Doctor activated successfully.",
                "Success");
    }

    public async Task<ApiResponse<string>>
        DeactivateAsync(Guid id)
    {
        var hospitalId =
            _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Hospital context not found.");
        }

        var doctor =
            await _repository.GetEntityByIdAsync(
                hospitalId,
                id);

        if (doctor == null)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Doctor not found.");
        }

        if (!doctor.IsActive)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Doctor is already inactive.");
        }

        await _repository.DeactivateAsync(
            hospitalId,
            id);

        return ApiResponse<string>
            .SuccessResponse(
                "Doctor deactivated successfully.",
                "Success");
    }

    public async Task<ApiResponse<string>>
    ResetPasswordAsync(Guid doctorId)
    {
        var hospitalId =
            _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Hospital context not found.");
        }

        var doctor =
            await _repository.GetEntityByIdAsync(
                hospitalId,
                doctorId);

        if (doctor == null)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Doctor not found.");
        }

        if (!doctor.UserId.HasValue)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Doctor login user not found.");
        }

        // =========================================================
        // DEVELOPMENT DEFAULT PASSWORD
        // =========================================================

        const string newPassword = "Doc@123";

        var passwordHash =
            _passwordHasher.HashPassword(
                newPassword);

        await _repository.ResetPasswordAsync(
            hospitalId,
            doctorId,
            passwordHash);

        return ApiResponse<string>
            .SuccessResponse(
                "Doctor password reset successfully. New password: Doc@123",
                "Success");
    }
}