using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Doctor.MedicalHistory;
using Healtive.Application.Interfaces;
using Healtive.Application.Interfaces.Repositories;

namespace Healtive.Infrastructure.Services.Doctors;

public class MedicalHistoryService : IMedicalHistoryService
{
    private readonly IMedicalHistoryRepository _repository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly ICurrentUserService _currentUserService;

    public MedicalHistoryService(
        IMedicalHistoryRepository repository,
        IDoctorRepository doctorRepository,
        ICurrentUserService currentUserService)
    {
        _repository = repository;
        _doctorRepository = doctorRepository;
        _currentUserService = currentUserService;
    }

    // ==========================================
    // GET ALL MEDICAL HISTORY FOR PATIENT
    // ==========================================

    public async Task<ApiResponse<IEnumerable<MedicalHistoryResponse>>> GetByPatientIdAsync(
        Guid patientId)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<IEnumerable<MedicalHistoryResponse>>
                .FailureResponse("Invalid user or hospital information.");
        }

        var doctor = await _doctorRepository.GetByUserIdAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<IEnumerable<MedicalHistoryResponse>>
                .FailureResponse("Doctor profile not found.");
        }

        var history = await _repository.GetByPatientIdAsync(
            hospitalId,
            doctor.Id,
            patientId);

        return ApiResponse<IEnumerable<MedicalHistoryResponse>>
            .SuccessResponse(history);
    }


    // ==========================================
    // GET MEDICAL HISTORY BY ID
    // ==========================================

    public async Task<ApiResponse<MedicalHistoryResponse>> GetByIdAsync(
        Guid historyId)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<MedicalHistoryResponse>
                .FailureResponse("Invalid user or hospital information.");
        }

        var doctor = await _doctorRepository.GetByUserIdAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<MedicalHistoryResponse>
                .FailureResponse("Doctor profile not found.");
        }

        var history = await _repository.GetByIdAsync(
            hospitalId,
            doctor.Id,
            historyId);

        if (history == null)
        {
            return ApiResponse<MedicalHistoryResponse>
                .FailureResponse("Medical history not found.");
        }

        return ApiResponse<MedicalHistoryResponse>
            .SuccessResponse(history);
    }


    // ==========================================
    // CREATE MEDICAL HISTORY
    // ==========================================

    public async Task<ApiResponse<MedicalHistoryResponse>> CreateAsync(
        CreateMedicalHistoryRequest request)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<MedicalHistoryResponse>
                .FailureResponse("Invalid user or hospital information.");
        }

        if (request.PatientId == Guid.Empty)
        {
            return ApiResponse<MedicalHistoryResponse>
                .FailureResponse("PatientId is required.");
        }

        if (string.IsNullOrWhiteSpace(request.MedicalCondition) &&
            string.IsNullOrWhiteSpace(request.Diagnosis) &&
            string.IsNullOrWhiteSpace(request.Treatment) &&
            string.IsNullOrWhiteSpace(request.Notes))
        {
            return ApiResponse<MedicalHistoryResponse>
                .FailureResponse(
                    "At least one medical history detail is required.");
        }

        var doctor = await _doctorRepository.GetByUserIdAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<MedicalHistoryResponse>
                .FailureResponse("Doctor profile not found.");
        }

        var history = await _repository.CreateAsync(
            hospitalId,
            doctor.Id,
            request);

        return ApiResponse<MedicalHistoryResponse>
            .SuccessResponse(
                history,
                "Medical history created successfully.");
    }


    // ==========================================
    // UPDATE MEDICAL HISTORY
    // ==========================================

    public async Task<ApiResponse<MedicalHistoryResponse>> UpdateAsync(
        Guid historyId,
        UpdateMedicalHistoryRequest request)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<MedicalHistoryResponse>
                .FailureResponse("Invalid user or hospital information.");
        }

        if (historyId == Guid.Empty)
        {
            return ApiResponse<MedicalHistoryResponse>
                .FailureResponse("Invalid medical history ID.");
        }

        if (string.IsNullOrWhiteSpace(request.MedicalCondition) &&
            string.IsNullOrWhiteSpace(request.Diagnosis) &&
            string.IsNullOrWhiteSpace(request.Treatment) &&
            string.IsNullOrWhiteSpace(request.Notes))
        {
            return ApiResponse<MedicalHistoryResponse>
                .FailureResponse(
                    "At least one medical history detail is required.");
        }

        var doctor = await _doctorRepository.GetByUserIdAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<MedicalHistoryResponse>
                .FailureResponse("Doctor profile not found.");
        }

        var existingHistory = await _repository.GetByIdAsync(
            hospitalId,
            doctor.Id,
            historyId);

        if (existingHistory == null)
        {
            return ApiResponse<MedicalHistoryResponse>
                .FailureResponse("Medical history not found.");
        }

        var updated = await _repository.UpdateAsync(
            hospitalId,
            doctor.Id,
            historyId,
            request);

        if (!updated)
        {
            return ApiResponse<MedicalHistoryResponse>
                .FailureResponse(
                    "Unable to update medical history.");
        }

        var history = await _repository.GetByIdAsync(
            hospitalId,
            doctor.Id,
            historyId);

        return ApiResponse<MedicalHistoryResponse>
            .SuccessResponse(
                history!,
                "Medical history updated successfully.");
    }


    // ==========================================
    // DELETE MEDICAL HISTORY
    // ==========================================

    public async Task<ApiResponse<string>> DeleteAsync(
        Guid historyId)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<string>
                .FailureResponse("Invalid user or hospital information.");
        }

        if (historyId == Guid.Empty)
        {
            return ApiResponse<string>
                .FailureResponse("Invalid medical history ID.");
        }

        var doctor = await _doctorRepository.GetByUserIdAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<string>
                .FailureResponse("Doctor profile not found.");
        }

        var existingHistory = await _repository.GetByIdAsync(
            hospitalId,
            doctor.Id,
            historyId);

        if (existingHistory == null)
        {
            return ApiResponse<string>
                .FailureResponse("Medical history not found.");
        }

        var deleted = await _repository.DeleteAsync(
            hospitalId,
            doctor.Id,
            historyId);

        if (!deleted)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Unable to delete medical history.");
        }

        return ApiResponse<string>
            .SuccessResponse(
                "Medical history deleted successfully.");
    }
}