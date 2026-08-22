using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Doctor;
using Healtive.Application.DTOs.DoctorSpecialization;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Healtive.Infrastructure.Services.Doctors;

public class DoctorSpecializationMappingService: IDoctorSpecializationMappingService
{
    private readonly IDoctorSpecializationMappingRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DoctorSpecializationMappingService(
        IDoctorSpecializationMappingRepository repository,
        IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
    }

    private Guid GetHospitalId()
    {
        var hospitalId =
            _httpContextAccessor.HttpContext?
                .User.FindFirst("HospitalId")?.Value;

        if (!Guid.TryParse(hospitalId, out var id))
        {
            throw new UnauthorizedAccessException(
                "Hospital information not found.");
        }

        return id;
    }

    public async Task<ApiResponse<bool>> AssignAsync(
        Guid doctorId,
        AssignDoctorSpecializationRequest request)
    {
        var hospitalId = GetHospitalId();

        var doctor = await _repository.GetDoctorAsync(
            hospitalId,
            doctorId);

        if (doctor == null)
        {
            return ApiResponse<bool>.FailureResponse(
                "Doctor not found.");
        }

        var specialization =
            await _repository.GetSpecializationAsync(
                request.SpecializationId);

        if (specialization == null)
        {
            return ApiResponse<bool>.FailureResponse(
                "Specialization not found.");
        }

        if (!specialization.IsActive)
        {
            return ApiResponse<bool>.FailureResponse(
                "Specialization is inactive.");
        }

        var exists =
            await _repository.MappingExistsAsync(
                doctorId,
                request.SpecializationId);

        if (exists)
        {
            return ApiResponse<bool>.FailureResponse(
                "Specialization is already assigned to this doctor.");
        }

        var mapping = new DoctorSpecializationMapping
        {
            DoctorId = doctorId,
            SpecializationId = request.SpecializationId,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AssignAsync(mapping);

        return ApiResponse<bool>.SuccessResponse(
            true,
            "Specialization assigned to doctor successfully.");
    }

    public async Task<
        ApiResponse<IEnumerable<DoctorSpecializationMappingResponse>>>
        GetDoctorSpecializationsAsync(
            Guid doctorId)
    {
        var hospitalId = GetHospitalId();

        var doctor = await _repository.GetDoctorAsync(
            hospitalId,
            doctorId);

        if (doctor == null)
        {
            return ApiResponse<
                IEnumerable<DoctorSpecializationMappingResponse>>
                .FailureResponse(
                    "Doctor not found.");
        }

        var result =
            await _repository.GetDoctorSpecializationsAsync(
                hospitalId,
                doctorId);

        return ApiResponse<
            IEnumerable<DoctorSpecializationMappingResponse>>
            .SuccessResponse(
                result,
                "Doctor specializations retrieved successfully.");
    }

    public async Task<ApiResponse<bool>> RemoveAsync(
        Guid doctorId,
        Guid specializationId)
    {
        var hospitalId = GetHospitalId();

        var doctor = await _repository.GetDoctorAsync(
            hospitalId,
            doctorId);

        if (doctor == null)
        {
            return ApiResponse<bool>.FailureResponse(
                "Doctor not found.");
        }

        var exists =
            await _repository.MappingExistsAsync(
                doctorId,
                specializationId);

        if (!exists)
        {
            return ApiResponse<bool>.FailureResponse(
                "Specialization is not assigned to this doctor.");
        }

        await _repository.RemoveAsync(
            doctorId,
            specializationId);

        return ApiResponse<bool>.SuccessResponse(
            true,
            "Specialization removed from doctor successfully.");
    }

    public async Task<
        ApiResponse<IEnumerable<DoctorListResponse>>>
        GetSpecializationDoctorsAsync(
            Guid specializationId)
    {
        var hospitalId = GetHospitalId();

        var specialization =
            await _repository.GetSpecializationAsync(
                specializationId);

        if (specialization == null)
        {
            return ApiResponse<
                IEnumerable<DoctorListResponse>>
                .FailureResponse(
                    "Specialization not found.");
        }

        var result =
            await _repository.GetSpecializationDoctorsAsync(
                hospitalId,
                specializationId);

        return ApiResponse<
            IEnumerable<DoctorListResponse>>
            .SuccessResponse(
                result,
                "Doctors retrieved successfully.");
    }
}