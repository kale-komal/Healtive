using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Doctor.Prescription;
using Healtive.Application.Interfaces;
using Healtive.Application.Interfaces.Repositories;
using Healtive.Core.Entities;

namespace Healtive.Infrastructure.Services.Doctors;

public class DoctorPrescriptionService : IDoctorPrescriptionService
{
    private readonly IDoctorPrescriptionRepository _repository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly ICurrentUserService _currentUserService;

    public DoctorPrescriptionService(
        IDoctorPrescriptionRepository repository,
        IDoctorRepository doctorRepository,
        ICurrentUserService currentUserService)
    {
        _repository = repository;
        _doctorRepository = doctorRepository;
        _currentUserService = currentUserService;
    }

    // =========================================================
    // GET PRESCRIPTIONS BY APPOINTMENT
    // =========================================================

    public async Task<ApiResponse<IEnumerable<PrescriptionResponse>>>
        GetByAppointmentIdAsync(
            Guid appointmentId)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<IEnumerable<PrescriptionResponse>>
                .FailureResponse("Invalid user or hospital information.");
        }

        if (appointmentId == Guid.Empty)
        {
            return ApiResponse<IEnumerable<PrescriptionResponse>>
                .FailureResponse("Invalid appointment ID.");
        }

        var doctor = await GetDoctorAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<IEnumerable<PrescriptionResponse>>
                .FailureResponse("Doctor profile not found.");
        }

        var prescriptions = await _repository.GetByAppointmentIdAsync(
            hospitalId,
            doctor.Id,
            appointmentId);

        return ApiResponse<IEnumerable<PrescriptionResponse>>
            .SuccessResponse(prescriptions);
    }

    // =========================================================
    // GET PRESCRIPTION BY ID
    // =========================================================

    public async Task<ApiResponse<PrescriptionResponse>>
        GetByIdAsync(
            Guid prescriptionId)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<PrescriptionResponse>
                .FailureResponse("Invalid user or hospital information.");
        }

        if (prescriptionId == Guid.Empty)
        {
            return ApiResponse<PrescriptionResponse>
                .FailureResponse("Invalid prescription ID.");
        }

        var doctor = await GetDoctorAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<PrescriptionResponse>
                .FailureResponse("Doctor profile not found.");
        }

        var prescription = await _repository.GetByIdAsync(
            hospitalId,
            doctor.Id,
            prescriptionId);

        if (prescription == null)
        {
            return ApiResponse<PrescriptionResponse>
                .FailureResponse("Prescription not found.");
        }

        return ApiResponse<PrescriptionResponse>
            .SuccessResponse(prescription);
    }

    // =========================================================
    // CREATE PRESCRIPTION
    // =========================================================

    public async Task<ApiResponse<PrescriptionResponse>>
        CreateAsync(
            CreatePrescriptionRequest request)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<PrescriptionResponse>
                .FailureResponse("Invalid user or hospital information.");
        }

        if (request.AppointmentId == Guid.Empty)
        {
            return ApiResponse<PrescriptionResponse>
                .FailureResponse("AppointmentId is required.");
        }

        var itemsError = ValidateItems(request.Items);

        if (itemsError != null)
        {
            return ApiResponse<PrescriptionResponse>
                .FailureResponse(itemsError);
        }

        var doctor = await GetDoctorAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<PrescriptionResponse>
                .FailureResponse("Doctor profile not found.");
        }

        try
        {
            var prescription = await _repository.CreateAsync(
                hospitalId,
                doctor.Id,
                request);

            return ApiResponse<PrescriptionResponse>
                .SuccessResponse(
                    prescription,
                    "Prescription created successfully.");
        }
        catch (InvalidOperationException ex)
        {
            return ApiResponse<PrescriptionResponse>
                .FailureResponse(ex.Message);
        }
    }

    // =========================================================
    // UPDATE PRESCRIPTION
    // =========================================================

    public async Task<ApiResponse<PrescriptionResponse>>
        UpdateAsync(
            Guid prescriptionId,
            UpdatePrescriptionRequest request)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<PrescriptionResponse>
                .FailureResponse("Invalid user or hospital information.");
        }

        if (prescriptionId == Guid.Empty)
        {
            return ApiResponse<PrescriptionResponse>
                .FailureResponse("Invalid prescription ID.");
        }

        var itemsError = ValidateItems(request.Items);

        if (itemsError != null)
        {
            return ApiResponse<PrescriptionResponse>
                .FailureResponse(itemsError);
        }

        var doctor = await GetDoctorAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<PrescriptionResponse>
                .FailureResponse("Doctor profile not found.");
        }

        var existing = await _repository.GetByIdAsync(
            hospitalId,
            doctor.Id,
            prescriptionId);

        if (existing == null)
        {
            return ApiResponse<PrescriptionResponse>
                .FailureResponse("Prescription not found.");
        }

        // =====================================================
        // FINALIZED PRESCRIPTIONS ARE READ-ONLY
        // =====================================================

        if (existing.IsFinalized)
        {
            return ApiResponse<PrescriptionResponse>
                .FailureResponse("Finalized prescription cannot be modified.");
        }

        try
        {
            var updated = await _repository.UpdateAsync(
                hospitalId,
                doctor.Id,
                prescriptionId,
                request);

            if (!updated)
            {
                return ApiResponse<PrescriptionResponse>
                    .FailureResponse("Unable to update prescription.");
            }

            var prescription = await _repository.GetByIdAsync(
                hospitalId,
                doctor.Id,
                prescriptionId);

            return ApiResponse<PrescriptionResponse>
                .SuccessResponse(
                    prescription!,
                    "Prescription updated successfully.");
        }
        catch (InvalidOperationException ex)
        {
            return ApiResponse<PrescriptionResponse>
                .FailureResponse(ex.Message);
        }
    }

    // =========================================================
    // DELETE PRESCRIPTION
    // =========================================================

    public async Task<ApiResponse<string>>
        DeleteAsync(
            Guid prescriptionId)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<string>
                .FailureResponse("Invalid user or hospital information.");
        }

        if (prescriptionId == Guid.Empty)
        {
            return ApiResponse<string>
                .FailureResponse("Invalid prescription ID.");
        }

        var doctor = await GetDoctorAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<string>
                .FailureResponse("Doctor profile not found.");
        }

        var existing = await _repository.GetByIdAsync(
            hospitalId,
            doctor.Id,
            prescriptionId);

        if (existing == null)
        {
            return ApiResponse<string>
                .FailureResponse("Prescription not found.");
        }

        // =====================================================
        // FINALIZED PRESCRIPTIONS CANNOT BE DELETED
        // =====================================================

        if (existing.IsFinalized)
        {
            return ApiResponse<string>
                .FailureResponse("Finalized prescription cannot be deleted.");
        }

        var deleted = await _repository.DeleteAsync(
            hospitalId,
            doctor.Id,
            prescriptionId);

        if (!deleted)
        {
            return ApiResponse<string>
                .FailureResponse("Unable to delete prescription.");
        }

        return ApiResponse<string>
            .SuccessResponse("Prescription deleted successfully.");
    }

    // =========================================================
    // FINALIZE PRESCRIPTION
    // =========================================================

    public async Task<ApiResponse<string>>
        FinalizeAsync(
            Guid prescriptionId)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<string>
                .FailureResponse("Invalid user or hospital information.");
        }

        if (prescriptionId == Guid.Empty)
        {
            return ApiResponse<string>
                .FailureResponse("Invalid prescription ID.");
        }

        var doctor = await GetDoctorAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<string>
                .FailureResponse("Doctor profile not found.");
        }

        var existing = await _repository.GetByIdAsync(
            hospitalId,
            doctor.Id,
            prescriptionId);

        if (existing == null)
        {
            return ApiResponse<string>
                .FailureResponse("Prescription not found.");
        }

        // =====================================================
        // ALREADY FINALIZED
        // =====================================================

        if (existing.IsFinalized)
        {
            return ApiResponse<string>
                .FailureResponse("Prescription is already finalized.");
        }

        // =====================================================
        // MUST CONTAIN AT LEAST ONE ITEM
        // =====================================================

        if (existing.Items.Count == 0)
        {
            return ApiResponse<string>
                .FailureResponse("Prescription must have at least one item to be finalized.");
        }

        var finalized = await _repository.FinalizeAsync(
            hospitalId,
            doctor.Id,
            prescriptionId);

        if (!finalized)
        {
            return ApiResponse<string>
                .FailureResponse("Unable to finalize prescription.");
        }

        return ApiResponse<string>
            .SuccessResponse("Prescription finalized successfully.");
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

    // =========================================================
    // VALIDATE ITEMS
    // =========================================================

    private static string? ValidateItems(
        List<PrescriptionItemRequest>? items)
    {
        if (items == null || items.Count == 0)
        {
            return "At least one prescription item is required.";
        }

        for (var i = 0; i < items.Count; i++)
        {
            var item = items[i];

            if (string.IsNullOrWhiteSpace(item.MedicineName))
            {
                return $"Medicine name is required for item {i + 1}.";
            }

            if (item.DosageId == Guid.Empty)
            {
                return $"Dosage is required for item {i + 1}.";
            }

            if (item.DurationDays < 1)
            {
                return $"Duration days must be at least 1 for item {i + 1}.";
            }

            if (item.Quantity <= 0)
            {
                return $"Quantity must be greater than zero for item {i + 1}.";
            }
        }

        return null;
    }
}