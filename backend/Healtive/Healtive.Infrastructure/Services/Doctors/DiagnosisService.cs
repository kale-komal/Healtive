using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Doctor.Diagnosis;
using Healtive.Application.Interfaces;
using Healtive.Application.Interfaces.Repositories;

namespace Healtive.Infrastructure.Services.Doctors;

public class DiagnosisService : IDiagnosisService
{
    private readonly IDiagnosisRepository _repository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly ICurrentUserService _currentUserService;

    public DiagnosisService(
        IDiagnosisRepository repository,
        IDoctorRepository doctorRepository,
        ICurrentUserService currentUserService)
    {
        _repository = repository;
        _doctorRepository = doctorRepository;
        _currentUserService = currentUserService;
    }

    // ==========================================
    // GET DIAGNOSES BY APPOINTMENT
    // ==========================================

    public async Task<ApiResponse<IEnumerable<DiagnosisResponse>>> GetByAppointmentIdAsync(
        Guid appointmentId)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<IEnumerable<DiagnosisResponse>>
                .FailureResponse("Invalid user or hospital information.");
        }

        if (appointmentId == Guid.Empty)
        {
            return ApiResponse<IEnumerable<DiagnosisResponse>>
                .FailureResponse("Invalid appointment ID.");
        }

        var doctor = await _doctorRepository.GetByUserIdAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<IEnumerable<DiagnosisResponse>>
                .FailureResponse("Doctor profile not found.");
        }

        var diagnoses = await _repository.GetByAppointmentIdAsync(
            hospitalId,
            doctor.Id,
            appointmentId);

        return ApiResponse<IEnumerable<DiagnosisResponse>>
            .SuccessResponse(diagnoses);
    }


    // ==========================================
    // GET DIAGNOSIS BY ID
    // ==========================================

    public async Task<ApiResponse<DiagnosisResponse>> GetByIdAsync(
        Guid diagnosisId)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<DiagnosisResponse>
                .FailureResponse("Invalid user or hospital information.");
        }

        if (diagnosisId == Guid.Empty)
        {
            return ApiResponse<DiagnosisResponse>
                .FailureResponse("Invalid diagnosis ID.");
        }

        var doctor = await _doctorRepository.GetByUserIdAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<DiagnosisResponse>
                .FailureResponse("Doctor profile not found.");
        }

        var diagnosis = await _repository.GetByIdAsync(
            hospitalId,
            doctor.Id,
            diagnosisId);

        if (diagnosis == null)
        {
            return ApiResponse<DiagnosisResponse>
                .FailureResponse("Diagnosis not found.");
        }

        return ApiResponse<DiagnosisResponse>
            .SuccessResponse(diagnosis);
    }


    // ==========================================
    // CREATE DIAGNOSIS
    // ==========================================

    public async Task<ApiResponse<DiagnosisResponse>> CreateAsync(
        CreateDiagnosisRequest request)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<DiagnosisResponse>
                .FailureResponse("Invalid user or hospital information.");
        }

        if (request.AppointmentId == Guid.Empty)
        {
            return ApiResponse<DiagnosisResponse>
                .FailureResponse("AppointmentId is required.");
        }

        if (string.IsNullOrWhiteSpace(request.DiagnosisName))
        {
            return ApiResponse<DiagnosisResponse>
                .FailureResponse("Diagnosis name is required.");
        }

        var doctor = await _doctorRepository.GetByUserIdAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<DiagnosisResponse>
                .FailureResponse("Doctor profile not found.");
        }

        try
        {
            var diagnosis = await _repository.CreateAsync(
                hospitalId,
                doctor.Id,
                request);

            return ApiResponse<DiagnosisResponse>
                .SuccessResponse(
                    diagnosis,
                    "Diagnosis created successfully.");
        }
        catch (InvalidOperationException ex)
        {
            return ApiResponse<DiagnosisResponse>
                .FailureResponse(ex.Message);
        }
    }


    // ==========================================
    // UPDATE DIAGNOSIS
    // ==========================================

    public async Task<ApiResponse<DiagnosisResponse>> UpdateAsync(
        Guid diagnosisId,
        UpdateDiagnosisRequest request)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<DiagnosisResponse>
                .FailureResponse("Invalid user or hospital information.");
        }

        if (diagnosisId == Guid.Empty)
        {
            return ApiResponse<DiagnosisResponse>
                .FailureResponse("Invalid diagnosis ID.");
        }

        if (string.IsNullOrWhiteSpace(request.DiagnosisName))
        {
            return ApiResponse<DiagnosisResponse>
                .FailureResponse("Diagnosis name is required.");
        }

        var doctor = await _doctorRepository.GetByUserIdAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<DiagnosisResponse>
                .FailureResponse("Doctor profile not found.");
        }

        var existingDiagnosis = await _repository.GetByIdAsync(
            hospitalId,
            doctor.Id,
            diagnosisId);

        if (existingDiagnosis == null)
        {
            return ApiResponse<DiagnosisResponse>
                .FailureResponse("Diagnosis not found.");
        }

        var updated = await _repository.UpdateAsync(
            hospitalId,
            doctor.Id,
            diagnosisId,
            request);

        if (!updated)
        {
            return ApiResponse<DiagnosisResponse>
                .FailureResponse("Unable to update diagnosis.");
        }

        var diagnosis = await _repository.GetByIdAsync(
            hospitalId,
            doctor.Id,
            diagnosisId);

        return ApiResponse<DiagnosisResponse>
            .SuccessResponse(
                diagnosis!,
                "Diagnosis updated successfully.");
    }


    // ==========================================
    // DELETE DIAGNOSIS
    // ==========================================

    public async Task<ApiResponse<string>> DeleteAsync(
        Guid diagnosisId)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<string>
                .FailureResponse("Invalid user or hospital information.");
        }

        if (diagnosisId == Guid.Empty)
        {
            return ApiResponse<string>
                .FailureResponse("Invalid diagnosis ID.");
        }

        var doctor = await _doctorRepository.GetByUserIdAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<string>
                .FailureResponse("Doctor profile not found.");
        }

        var existingDiagnosis = await _repository.GetByIdAsync(
            hospitalId,
            doctor.Id,
            diagnosisId);

        if (existingDiagnosis == null)
        {
            return ApiResponse<string>
                .FailureResponse("Diagnosis not found.");
        }

        var deleted = await _repository.DeleteAsync(
            hospitalId,
            doctor.Id,
            diagnosisId);

        if (!deleted)
        {
            return ApiResponse<string>
                .FailureResponse("Unable to delete diagnosis.");
        }

        return ApiResponse<string>
            .SuccessResponse(
                "Diagnosis deleted successfully.");
    }
}