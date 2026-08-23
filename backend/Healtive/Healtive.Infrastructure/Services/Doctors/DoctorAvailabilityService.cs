using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.DoctorAvailability;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Healtive.Infrastructure.Services.Doctors;

public class DoctorAvailabilityService : IDoctorAvailabilityService
{
    private readonly IDoctorAvailabilityRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DoctorAvailabilityService(
        IDoctorAvailabilityRepository repository,
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

    public async Task<ApiResponse<DoctorAvailabilityResponse>> CreateAsync(
        Guid doctorId,
        CreateDoctorAvailabilityRequest request)
    {
        var hospitalId = GetHospitalId();

        // Validate day
        if (request.DayOfWeek > 6)
        {
            return ApiResponse<DoctorAvailabilityResponse>
                .FailureResponse(
                    "DayOfWeek must be between 0 and 6.");
        }

        // Validate time
        if (request.StartTime >= request.EndTime)
        {
            return ApiResponse<DoctorAvailabilityResponse>
                .FailureResponse(
                    "Start time must be earlier than end time.");
        }

        // Validate appointments
        if (request.MaxAppointments <= 0)
        {
            return ApiResponse<DoctorAvailabilityResponse>
                .FailureResponse(
                    "Max appointments must be greater than 0.");
        }

        // Check doctor
        var doctor = await _repository.GetDoctorAsync(
            hospitalId,
            doctorId);

        if (doctor == null)
        {
            return ApiResponse<DoctorAvailabilityResponse>
                .FailureResponse(
                    "Doctor not found.");
        }

        if (!doctor.IsActive)
        {
            return ApiResponse<DoctorAvailabilityResponse>
                .FailureResponse(
                    "Doctor is inactive.");
        }

        // Check overlap
        var hasOverlap = await _repository.HasOverlapAsync(
            doctorId,
            request.DayOfWeek,
            request.StartTime,
            request.EndTime);

        if (hasOverlap)
        {
            return ApiResponse<DoctorAvailabilityResponse>
                .FailureResponse(
                    "Doctor already has an overlapping availability for this day and time.");
        }

        var availability = new DoctorAvailability
        {
            Id = Guid.NewGuid(),
            DoctorId = doctorId,
            DayOfWeek = request.DayOfWeek,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            MaxAppointments = request.MaxAppointments,
            IsAvailable = true,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.CreateAsync(availability);

        var response = new DoctorAvailabilityResponse
        {
            Id = availability.Id,
            DoctorId = availability.DoctorId,
            DayOfWeek = availability.DayOfWeek,
            StartTime = availability.StartTime,
            EndTime = availability.EndTime,
            MaxAppointments = availability.MaxAppointments,
            IsAvailable = availability.IsAvailable,
            CreatedAt = availability.CreatedAt
        };

        return ApiResponse<DoctorAvailabilityResponse>
            .SuccessResponse(
                response,
                "Doctor availability created successfully.");
    }

    public async Task<ApiResponse<DoctorAvailabilityResponse>> UpdateAsync(
        Guid doctorId,
        Guid id,
        UpdateDoctorAvailabilityRequest request)
    {
        var hospitalId = GetHospitalId();

        if (request.DayOfWeek > 6)
        {
            return ApiResponse<DoctorAvailabilityResponse>
                .FailureResponse(
                    "DayOfWeek must be between 0 and 6.");
        }

        if (request.StartTime >= request.EndTime)
        {
            return ApiResponse<DoctorAvailabilityResponse>
                .FailureResponse(
                    "Start time must be earlier than end time.");
        }

        if (request.MaxAppointments <= 0)
        {
            return ApiResponse<DoctorAvailabilityResponse>
                .FailureResponse(
                    "Max appointments must be greater than 0.");
        }

        var doctor = await _repository.GetDoctorAsync(
            hospitalId,
            doctorId);

        if (doctor == null)
        {
            return ApiResponse<DoctorAvailabilityResponse>
                .FailureResponse(
                    "Doctor not found.");
        }

        if (!doctor.IsActive)
        {
            return ApiResponse<DoctorAvailabilityResponse>
                .FailureResponse(
                    "Doctor is inactive.");
        }

        var existing = await _repository.GetByIdAsync(
            doctorId,
            id);

        if (existing == null)
        {
            return ApiResponse<DoctorAvailabilityResponse>
                .FailureResponse(
                    "Doctor availability not found.");
        }

        var hasOverlap = await _repository.HasOverlapAsync(
            doctorId,
            request.DayOfWeek,
            request.StartTime,
            request.EndTime,
            id);

        if (hasOverlap)
        {
            return ApiResponse<DoctorAvailabilityResponse>
                .FailureResponse(
                    "Doctor already has an overlapping availability for this day and time.");
        }

        existing.DayOfWeek = request.DayOfWeek;
        existing.StartTime = request.StartTime;
        existing.EndTime = request.EndTime;
        existing.MaxAppointments = request.MaxAppointments;
        existing.IsAvailable = request.IsAvailable;

        await _repository.UpdateAsync(existing);

        var response = new DoctorAvailabilityResponse
        {
            Id = existing.Id,
            DoctorId = existing.DoctorId,
            DayOfWeek = existing.DayOfWeek,
            StartTime = existing.StartTime,
            EndTime = existing.EndTime,
            MaxAppointments = existing.MaxAppointments,
            IsAvailable = existing.IsAvailable,
            CreatedAt = existing.CreatedAt
        };

        return ApiResponse<DoctorAvailabilityResponse>
            .SuccessResponse(
                response,
                "Doctor availability updated successfully.");
    }

    public async Task<ApiResponse<IEnumerable<DoctorAvailabilityResponse>>>
        GetByDoctorAsync(Guid doctorId)
    {
        var hospitalId = GetHospitalId();

        var doctor = await _repository.GetDoctorAsync(
            hospitalId,
            doctorId);

        if (doctor == null)
        {
            return ApiResponse<IEnumerable<DoctorAvailabilityResponse>>
                .FailureResponse(
                    "Doctor not found.");
        }

        var result = await _repository.GetByDoctorAsync(
            hospitalId,
            doctorId);

        return ApiResponse<IEnumerable<DoctorAvailabilityResponse>>
            .SuccessResponse(
                result,
                "Doctor availability retrieved successfully.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(
        Guid doctorId,
        Guid id)
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

        var existing = await _repository.GetByIdAsync(
            doctorId,
            id);

        if (existing == null)
        {
            return ApiResponse<bool>.FailureResponse(
                "Doctor availability not found.");
        }

        await _repository.DeleteAsync(
            doctorId,
            id);

        return ApiResponse<bool>.SuccessResponse(
            true,
            "Doctor availability deleted successfully.");
    }

    public async Task<ApiResponse<bool>> ActivateAsync(
        Guid doctorId,
        Guid id)
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

        var existing = await _repository.GetByIdAsync(
            doctorId,
            id);

        if (existing == null)
        {
            return ApiResponse<bool>.FailureResponse(
                "Doctor availability not found.");
        }

        await _repository.ActivateAsync(
            doctorId,
            id);

        return ApiResponse<bool>.SuccessResponse(
            true,
            "Doctor availability activated successfully.");
    }

    public async Task<ApiResponse<bool>> DeactivateAsync(
        Guid doctorId,
        Guid id)
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

        var existing = await _repository.GetByIdAsync(
            doctorId,
            id);

        if (existing == null)
        {
            return ApiResponse<bool>.FailureResponse(
                "Doctor availability not found.");
        }

        await _repository.DeactivateAsync(
            doctorId,
            id);

        return ApiResponse<bool>.SuccessResponse(
            true,
            "Doctor availability deactivated successfully.");
    }
}