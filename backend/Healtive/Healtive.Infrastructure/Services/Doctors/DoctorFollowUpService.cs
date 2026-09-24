using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Doctor.FollowUp;
using Healtive.Application.Interfaces;
using Healtive.Application.Interfaces.Repositories;
using Healtive.Core.Entities;

namespace Healtive.Infrastructure.Services.Doctors;

public class DoctorFollowUpService : IDoctorFollowUpService
{
    private readonly IDoctorFollowUpRepository _repository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly ICurrentUserService _currentUserService;

    public DoctorFollowUpService(
        IDoctorFollowUpRepository repository,
        IDoctorRepository doctorRepository,
        ICurrentUserService currentUserService)
    {
        _repository = repository;
        _doctorRepository = doctorRepository;
        _currentUserService = currentUserService;
    }

    // =========================================================
    // GET UPCOMING FOLLOW-UPS
    // =========================================================

    public async Task<ApiResponse<IEnumerable<FollowUpResponse>>>
        GetUpcomingAsync()
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<IEnumerable<FollowUpResponse>>
                .FailureResponse("Invalid user or hospital information.");
        }

        var doctor = await GetDoctorAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<IEnumerable<FollowUpResponse>>
                .FailureResponse("Doctor profile not found.");
        }

        var followUps = await _repository.GetUpcomingAsync(
            hospitalId,
            doctor.Id);

        return ApiResponse<IEnumerable<FollowUpResponse>>
            .SuccessResponse(followUps);
    }

    // =========================================================
    // GET FOLLOW-UPS BY APPOINTMENT
    // =========================================================

    public async Task<ApiResponse<IEnumerable<FollowUpResponse>>>
        GetByAppointmentIdAsync(
            Guid appointmentId)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<IEnumerable<FollowUpResponse>>
                .FailureResponse("Invalid user or hospital information.");
        }

        if (appointmentId == Guid.Empty)
        {
            return ApiResponse<IEnumerable<FollowUpResponse>>
                .FailureResponse("Invalid appointment ID.");
        }

        var doctor = await GetDoctorAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<IEnumerable<FollowUpResponse>>
                .FailureResponse("Doctor profile not found.");
        }

        var followUps = await _repository.GetByAppointmentIdAsync(
            hospitalId,
            doctor.Id,
            appointmentId);

        return ApiResponse<IEnumerable<FollowUpResponse>>
            .SuccessResponse(followUps);
    }

    // =========================================================
    // GET FOLLOW-UP BY ID
    // =========================================================

    public async Task<ApiResponse<FollowUpResponse>>
        GetByIdAsync(
            Guid followUpId)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Invalid user or hospital information.");
        }

        if (followUpId == Guid.Empty)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Invalid follow-up ID.");
        }

        var doctor = await GetDoctorAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Doctor profile not found.");
        }

        var followUp = await _repository.GetByIdAsync(
            hospitalId,
            doctor.Id,
            followUpId);

        if (followUp == null)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Follow-up not found.");
        }

        return ApiResponse<FollowUpResponse>
            .SuccessResponse(followUp);
    }

    // =========================================================
    // CREATE FOLLOW-UP
    // =========================================================

    public async Task<ApiResponse<FollowUpResponse>>
        CreateAsync(
            CreateFollowUpRequest request)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Invalid user or hospital information.");
        }

        if (request == null)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Invalid follow-up request.");
        }

        if (request.AppointmentId == Guid.Empty)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("AppointmentId is required.");
        }

        if (request.FollowUpDate < DateOnly.FromDateTime(DateTime.Today))
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Follow-up date cannot be in the past.");
        }

        var doctor = await GetDoctorAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Doctor profile not found.");
        }

        return await _repository.CreateAsync(
            hospitalId,
            doctor.Id,
            request);
    }

    // =========================================================
    // UPDATE FOLLOW-UP
    // =========================================================

    public async Task<ApiResponse<FollowUpResponse>>
        UpdateAsync(
            Guid followUpId,
            UpdateFollowUpRequest request)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Invalid user or hospital information.");
        }

        if (followUpId == Guid.Empty)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Invalid follow-up ID.");
        }

        if (request == null)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Invalid follow-up request.");
        }

        if (request.FollowUpDate < DateOnly.FromDateTime(DateTime.Today))
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Follow-up date cannot be in the past.");
        }

        if (request.Status != "Pending"
            && request.Status != "Completed"
            && request.Status != "Cancelled")
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Invalid follow-up status.");
        }

        var doctor = await GetDoctorAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Doctor profile not found.");
        }

        var existing = await _repository.GetByIdAsync(
            hospitalId,
            doctor.Id,
            followUpId);

        if (existing == null)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Follow-up not found.");
        }

        // =====================================================
        // COMPLETED OR CANCELLED FOLLOW-UPS ARE READ-ONLY
        // =====================================================

        if (existing.Status == "Completed")
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Completed follow-up cannot be updated.");
        }

        if (existing.Status == "Cancelled")
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Cancelled follow-up cannot be updated.");
        }

        // =====================================================
        // LIFECYCLE TRANSITIONS BELONG TO DEDICATED ENDPOINTS
        // =====================================================

        if (request.Status == "Completed")
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Use the complete endpoint to complete a follow-up.");
        }

        if (request.Status == "Cancelled")
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Use the cancel endpoint to cancel a follow-up.");
        }

        return await _repository.UpdateAsync(
            hospitalId,
            doctor.Id,
            followUpId,
            request);
    }

    // =========================================================
    // COMPLETE FOLLOW-UP
    // =========================================================

    public async Task<ApiResponse<FollowUpResponse>>
        CompleteAsync(
            Guid followUpId)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Invalid user or hospital information.");
        }

        if (followUpId == Guid.Empty)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Invalid follow-up ID.");
        }

        var doctor = await GetDoctorAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Doctor profile not found.");
        }

        var existing = await _repository.GetByIdAsync(
            hospitalId,
            doctor.Id,
            followUpId);

        if (existing == null)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Follow-up not found.");
        }

        if (existing.Status == "Completed")
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Follow-up is already completed.");
        }

        if (existing.Status == "Cancelled")
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Cancelled follow-up cannot be completed.");
        }

        return await _repository.CompleteAsync(
            hospitalId,
            doctor.Id,
            followUpId);
    }

    // =========================================================
    // CANCEL FOLLOW-UP
    // =========================================================

    public async Task<ApiResponse<FollowUpResponse>>
        CancelAsync(
            Guid followUpId)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Invalid user or hospital information.");
        }

        if (followUpId == Guid.Empty)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Invalid follow-up ID.");
        }

        var doctor = await GetDoctorAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Doctor profile not found.");
        }

        var existing = await _repository.GetByIdAsync(
            hospitalId,
            doctor.Id,
            followUpId);

        if (existing == null)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Follow-up not found.");
        }

        if (existing.Status == "Cancelled")
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Follow-up is already cancelled.");
        }

        if (existing.Status == "Completed")
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Completed follow-up cannot be cancelled.");
        }

        return await _repository.CancelAsync(
            hospitalId,
            doctor.Id,
            followUpId);
    }

    // =========================================================
    // GET DOCTOR
    // =========================================================

    private async Task<Doctor?>
        GetDoctorAsync(
            Guid hospitalId,
            Guid userId)
    {
        return await _doctorRepository.GetByUserIdAsync(
            hospitalId,
            userId);
    }
}