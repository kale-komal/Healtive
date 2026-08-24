using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.DoctorLeave;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Healtive.Infrastructure.Services.Doctors;

public class DoctorLeaveService : IDoctorLeaveService
{
    private readonly IDoctorLeaveRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DoctorLeaveService(
        IDoctorLeaveRepository repository,
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

    public async Task<ApiResponse<DoctorLeaveResponse>> CreateAsync(
        Guid doctorId,
        CreateDoctorLeaveRequest request)
    {
        var hospitalId = GetHospitalId();

        if (request.FromDate > request.ToDate)
        {
            return ApiResponse<DoctorLeaveResponse>
                .FailureResponse(
                    "From date must be earlier than or equal to the to date.");
        }

        var doctor = await _repository.GetDoctorAsync(
            hospitalId,
            doctorId);

        if (doctor == null)
        {
            return ApiResponse<DoctorLeaveResponse>
                .FailureResponse(
                    "Doctor not found.");
        }

        if (!doctor.IsActive)
        {
            return ApiResponse<DoctorLeaveResponse>
                .FailureResponse(
                    "Doctor is inactive.");
        }

        var hasOverlap = await _repository.HasOverlapAsync(
            doctorId,
            request.FromDate,
            request.ToDate);

        if (hasOverlap)
        {
            return ApiResponse<DoctorLeaveResponse>
                .FailureResponse(
                    "Doctor already has leave during the selected dates.");
        }

        var leave = new DoctorLeave
        {
            Id = Guid.NewGuid(),
            DoctorId = doctorId,
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            Reason = request.Reason,
            IsApproved = false,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.CreateAsync(leave);

        var response = new DoctorLeaveResponse
        {
            Id = leave.Id,
            DoctorId = leave.DoctorId,
            FromDate = leave.FromDate,
            ToDate = leave.ToDate,
            Reason = leave.Reason,
            IsApproved = leave.IsApproved,
            CreatedAt = leave.CreatedAt
        };

        return ApiResponse<DoctorLeaveResponse>
            .SuccessResponse(
                response,
                "Doctor leave created successfully and is pending approval.");
    }

    public async Task<ApiResponse<DoctorLeaveResponse>> UpdateAsync(
        Guid doctorId,
        Guid id,
        UpdateDoctorLeaveRequest request)
    {
        var hospitalId = GetHospitalId();

        if (request.FromDate > request.ToDate)
        {
            return ApiResponse<DoctorLeaveResponse>
                .FailureResponse(
                    "From date must be earlier than or equal to the to date.");
        }

        var doctor = await _repository.GetDoctorAsync(
            hospitalId,
            doctorId);

        if (doctor == null)
        {
            return ApiResponse<DoctorLeaveResponse>
                .FailureResponse(
                    "Doctor not found.");
        }

        if (!doctor.IsActive)
        {
            return ApiResponse<DoctorLeaveResponse>
                .FailureResponse(
                    "Doctor is inactive.");
        }

        var existing = await _repository.GetByIdAsync(
            doctorId,
            id);

        if (existing == null)
        {
            return ApiResponse<DoctorLeaveResponse>
                .FailureResponse(
                    "Doctor leave not found.");
        }

        var hasOverlap = await _repository.HasOverlapAsync(
            doctorId,
            request.FromDate,
            request.ToDate,
            id);

        if (hasOverlap)
        {
            return ApiResponse<DoctorLeaveResponse>
                .FailureResponse(
                    "Doctor already has another leave during the selected dates.");
        }

        existing.FromDate = request.FromDate;
        existing.ToDate = request.ToDate;
        existing.Reason = request.Reason;

        // Changing leave details requires approval again.
        existing.IsApproved = false;

        await _repository.UpdateAsync(existing);

        var response = new DoctorLeaveResponse
        {
            Id = existing.Id,
            DoctorId = existing.DoctorId,
            FromDate = existing.FromDate,
            ToDate = existing.ToDate,
            Reason = existing.Reason,
            IsApproved = existing.IsApproved,
            CreatedAt = existing.CreatedAt
        };

        return ApiResponse<DoctorLeaveResponse>
            .SuccessResponse(
                response,
                "Doctor leave updated successfully and is pending approval.");
    }

    public async Task<ApiResponse<IEnumerable<DoctorLeaveResponse>>>
        GetByDoctorAsync(Guid doctorId)
    {
        var hospitalId = GetHospitalId();

        var doctor = await _repository.GetDoctorAsync(
            hospitalId,
            doctorId);

        if (doctor == null)
        {
            return ApiResponse<IEnumerable<DoctorLeaveResponse>>
                .FailureResponse(
                    "Doctor not found.");
        }

        var result = await _repository.GetByDoctorAsync(
            hospitalId,
            doctorId);

        return ApiResponse<IEnumerable<DoctorLeaveResponse>>
            .SuccessResponse(
                result,
                "Doctor leaves retrieved successfully.");
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
            return ApiResponse<bool>
                .FailureResponse(
                    "Doctor not found.");
        }

        var existing = await _repository.GetByIdAsync(
            doctorId,
            id);

        if (existing == null)
        {
            return ApiResponse<bool>
                .FailureResponse(
                    "Doctor leave not found.");
        }

        await _repository.DeleteAsync(
            doctorId,
            id);

        return ApiResponse<bool>
            .SuccessResponse(
                true,
                "Doctor leave deleted successfully.");
    }

    public async Task<ApiResponse<bool>> ApproveAsync(
        Guid doctorId,
        Guid id)
    {
        var hospitalId = GetHospitalId();

        var doctor = await _repository.GetDoctorAsync(
            hospitalId,
            doctorId);

        if (doctor == null)
        {
            return ApiResponse<bool>
                .FailureResponse(
                    "Doctor not found.");
        }

        var existing = await _repository.GetByIdAsync(
            doctorId,
            id);

        if (existing == null)
        {
            return ApiResponse<bool>
                .FailureResponse(
                    "Doctor leave not found.");
        }

        if (existing.IsApproved)
        {
            return ApiResponse<bool>
                .FailureResponse(
                    "Doctor leave is already approved.");
        }

        await _repository.ApproveAsync(
            doctorId,
            id);

        return ApiResponse<bool>
            .SuccessResponse(
                true,
                "Doctor leave approved successfully.");
    }

    public async Task<ApiResponse<bool>> RejectAsync(
        Guid doctorId,
        Guid id)
    {
        var hospitalId = GetHospitalId();

        var doctor = await _repository.GetDoctorAsync(
            hospitalId,
            doctorId);

        if (doctor == null)
        {
            return ApiResponse<bool>
                .FailureResponse(
                    "Doctor not found.");
        }

        var existing = await _repository.GetByIdAsync(
            doctorId,
            id);

        if (existing == null)
        {
            return ApiResponse<bool>
                .FailureResponse(
                    "Doctor leave not found.");
        }

        if (!existing.IsApproved)
        {
            return ApiResponse<bool>
                .FailureResponse(
                    "Doctor leave is already rejected or pending.");
        }

        await _repository.RejectAsync(
            doctorId,
            id);

        return ApiResponse<bool>
            .SuccessResponse(
                true,
                "Doctor leave rejected successfully.");
    }
}