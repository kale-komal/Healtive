using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Doctor;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;

namespace Healtive.Infrastructure.Services.Doctors;

public class DoctorDepartmentService : IDoctorDepartmentService
{
    private readonly IDoctorDepartmentRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public DoctorDepartmentService(
        IDoctorDepartmentRepository repository,
        ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<ApiResponse<string>> AssignAsync(
        Guid doctorId,
        AssignDoctorDepartmentRequest request)
    {
        var hospitalId = _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Hospital context not found.");
        }

        if (doctorId == Guid.Empty)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Invalid doctor.");
        }

        if (request.DepartmentId == Guid.Empty)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Department is required.");
        }

        // Check doctor belongs to current hospital
        var doctor = await _repository.GetDoctorAsync(
            hospitalId,
            doctorId);

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
                    "Cannot assign department to an inactive doctor.");
        }

        // Check department belongs to current hospital
        var department = await _repository.GetDepartmentAsync(
            hospitalId,
            request.DepartmentId);

        if (department == null)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Department not found.");
        }

        if (!department.IsActive)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Cannot assign an inactive department.");
        }

        // Prevent duplicate mapping
        var alreadyExists =
            await _repository.MappingExistsAsync(
                doctorId,
                request.DepartmentId);

        if (alreadyExists)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Doctor is already assigned to this department.");
        }

        var mapping = new DoctorDepartment
        {
            DoctorId = doctorId,
            DepartmentId = request.DepartmentId,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AssignAsync(mapping);

        return ApiResponse<string>
            .SuccessResponse(
                "Department assigned to doctor successfully.",
                "Success");
    }

    public async Task<ApiResponse<IEnumerable<DoctorDepartmentResponse>>>
        GetDoctorDepartmentsAsync(
            Guid doctorId)
    {
        var hospitalId = _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<IEnumerable<DoctorDepartmentResponse>>
                .FailureResponse(
                    "Hospital context not found.");
        }

        var doctor = await _repository.GetDoctorAsync(
            hospitalId,
            doctorId);

        if (doctor == null)
        {
            return ApiResponse<IEnumerable<DoctorDepartmentResponse>>
                .FailureResponse(
                    "Doctor not found.");
        }

        var departments =
            await _repository.GetDoctorDepartmentsAsync(
                hospitalId,
                doctorId);

        return ApiResponse<IEnumerable<DoctorDepartmentResponse>>
            .SuccessResponse(
                departments,
                "Doctor departments fetched successfully.");
    }

    public async Task<ApiResponse<string>> RemoveAsync(
        Guid doctorId,
        Guid departmentId)
    {
        var hospitalId = _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Hospital context not found.");
        }

        var doctor = await _repository.GetDoctorAsync(
            hospitalId,
            doctorId);

        if (doctor == null)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Doctor not found.");
        }

        var department = await _repository.GetDepartmentAsync(
            hospitalId,
            departmentId);

        if (department == null)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Department not found.");
        }

        var exists =
            await _repository.MappingExistsAsync(
                doctorId,
                departmentId);

        if (!exists)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Doctor is not assigned to this department.");
        }

        await _repository.RemoveAsync(
            doctorId,
            departmentId);

        return ApiResponse<string>
            .SuccessResponse(
                "Department removed from doctor successfully.",
                "Success");
    }

    public async Task<ApiResponse<IEnumerable<DoctorListResponse>>>
        GetDepartmentDoctorsAsync(
            Guid departmentId)
    {
        var hospitalId = _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<IEnumerable<DoctorListResponse>>
                .FailureResponse(
                    "Hospital context not found.");
        }

        var department = await _repository.GetDepartmentAsync(
            hospitalId,
            departmentId);

        if (department == null)
        {
            return ApiResponse<IEnumerable<DoctorListResponse>>
                .FailureResponse(
                    "Department not found.");
        }

        var doctors =
            await _repository.GetDepartmentDoctorsAsync(
                hospitalId,
                departmentId);

        return ApiResponse<IEnumerable<DoctorListResponse>>
            .SuccessResponse(
                doctors,
                "Department doctors fetched successfully.");
    }
}