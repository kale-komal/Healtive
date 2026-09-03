using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Doctor.Consultation;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;

namespace Healtive.Infrastructure.Services.Doctors;

public class ConsultationService : IConsultationService
{
    private readonly IConsultationRepository _repository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDoctorRepository _doctorRepository;

    public ConsultationService(
    IConsultationRepository repository,
    IDoctorRepository doctorRepository,
    ICurrentUserService currentUserService)
    {
        _repository = repository;
        _doctorRepository = doctorRepository;
        _currentUserService = currentUserService;
    }

    // =========================================================
    // CREATE CONSULTATION
    // =========================================================

    public async Task<ApiResponse<ConsultationResponse>>
        CreateAsync(
            CreateConsultationRequest request)
    {
        var hospitalId =
            _currentUserService.HospitalId;

        var doctorId =
            _currentUserService.UserId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<ConsultationResponse>
                .FailureResponse(
                    "Hospital context not found.");
        }

        if (doctorId == Guid.Empty)
        {
            return ApiResponse<ConsultationResponse>
                .FailureResponse(
                    "Doctor context not found.");
        }

        if (request.AppointmentId == Guid.Empty)
        {
            return ApiResponse<ConsultationResponse>
                .FailureResponse(
                    "Appointment is required.");
        }

        // =====================================================
        // IMPORTANT
        // CurrentUserService.UserId is the logged-in USER ID,
        // not the Doctor table ID.
        //
        // We therefore need the Doctor ID from the user.
        // =====================================================

        var doctor =
            await GetDoctorAsync(
                hospitalId,
                doctorId);

        if (doctor == null)
        {
            return ApiResponse<ConsultationResponse>
                .FailureResponse(
                    "Doctor profile not found.");
        }

        var actualDoctorId = doctor.Id;

        // =====================================================
        // VERIFY APPOINTMENT
        // =====================================================

        var appointment =
            await _repository
                .GetAppointmentForConsultationAsync(
                    hospitalId,
                    actualDoctorId,
                    request.AppointmentId);

        if (appointment == null)
        {
            return ApiResponse<ConsultationResponse>
                .FailureResponse(
                    "Appointment not found or does not belong to this doctor.");
        }

        // =====================================================
        // CHECK DUPLICATE CONSULTATION
        // =====================================================

        if (await _repository.ExistsByAppointmentIdAsync(
                request.AppointmentId))
        {
            return ApiResponse<ConsultationResponse>
                .FailureResponse(
                    "Consultation already exists for this appointment.");
        }

        // =====================================================
        // VALIDATION
        // =====================================================

        if (string.IsNullOrWhiteSpace(
                request.ChiefComplaint) &&
            string.IsNullOrWhiteSpace(
                request.ClinicalNotes) &&
            string.IsNullOrWhiteSpace(
                request.ExaminationNotes) &&
            string.IsNullOrWhiteSpace(
                request.TreatmentNotes) &&
            string.IsNullOrWhiteSpace(
                request.Advice))
        {
            return ApiResponse<ConsultationResponse>
                .FailureResponse(
                    "At least one consultation detail is required.");
        }

        var now = DateTime.UtcNow;

        var consultation = new Consultation
        {
            Id = Guid.NewGuid(),

            HospitalId = hospitalId,

            AppointmentId =
                request.AppointmentId,

            PatientId =
                appointment.PatientId,

            DoctorId =
                actualDoctorId,

            ConsultationDate =
                appointment.ConsultationDate,

            ChiefComplaint =
                CleanText(request.ChiefComplaint),

            ClinicalNotes =
                CleanText(request.ClinicalNotes),

            ExaminationNotes =
                CleanText(request.ExaminationNotes),

            TreatmentNotes =
                CleanText(request.TreatmentNotes),

            Advice =
                CleanText(request.Advice),

            IsCompleted = false,

            CompletedAt = null,

            CreatedAt = now,

            UpdatedAt = null
        };

        await _repository.CreateAsync(
            consultation);

        // =====================================================
        // GET CREATED CONSULTATION
        // =====================================================

        var response =
            await _repository.GetByAppointmentIdAsync(
                hospitalId,
                actualDoctorId,
                request.AppointmentId);

        if (response == null)
        {
            return ApiResponse<ConsultationResponse>
                .FailureResponse(
                    "Consultation was created but could not be retrieved.");
        }

        return ApiResponse<ConsultationResponse>
            .SuccessResponse(
                response,
                "Consultation created successfully.");
    }

    // =========================================================
    // GET CONSULTATION
    // =========================================================

    public async Task<ApiResponse<ConsultationResponse>>
        GetByAppointmentIdAsync(
            Guid appointmentId)
    {
        var hospitalId =
            _currentUserService.HospitalId;

        var userId =
            _currentUserService.UserId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<ConsultationResponse>
                .FailureResponse(
                    "Hospital context not found.");
        }

        if (userId == Guid.Empty)
        {
            return ApiResponse<ConsultationResponse>
                .FailureResponse(
                    "User context not found.");
        }

        var doctor =
            await GetDoctorAsync(
                hospitalId,
                userId);

        if (doctor == null)
        {
            return ApiResponse<ConsultationResponse>
                .FailureResponse(
                    "Doctor profile not found.");
        }

        var response =
            await _repository.GetByAppointmentIdAsync(
                hospitalId,
                doctor.Id,
                appointmentId);

        if (response == null)
        {
            return ApiResponse<ConsultationResponse>
                .FailureResponse(
                    "Consultation not found.");
        }

        return ApiResponse<ConsultationResponse>
            .SuccessResponse(
                response,
                "Consultation fetched successfully.");
    }

    // =========================================================
    // UPDATE CONSULTATION
    // =========================================================

    public async Task<ApiResponse<ConsultationResponse>>
        UpdateAsync(
            Guid consultationId,
            UpdateConsultationRequest request)
    {
        var hospitalId =
            _currentUserService.HospitalId;

        var userId =
            _currentUserService.UserId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<ConsultationResponse>
                .FailureResponse(
                    "Hospital context not found.");
        }

        if (userId == Guid.Empty)
        {
            return ApiResponse<ConsultationResponse>
                .FailureResponse(
                    "User context not found.");
        }

        if (consultationId == Guid.Empty)
        {
            return ApiResponse<ConsultationResponse>
                .FailureResponse(
                    "Consultation ID is required.");
        }

        var doctor =
            await GetDoctorAsync(
                hospitalId,
                userId);

        if (doctor == null)
        {
            return ApiResponse<ConsultationResponse>
                .FailureResponse(
                    "Doctor profile not found.");
        }

        var consultation =
            await _repository.GetEntityByIdAsync(
                hospitalId,
                doctor.Id,
                consultationId);

        if (consultation == null)
        {
            return ApiResponse<ConsultationResponse>
                .FailureResponse(
                    "Consultation not found.");
        }

        if (consultation.IsCompleted)
        {
            return ApiResponse<ConsultationResponse>
                .FailureResponse(
                    "Completed consultation cannot be modified.");
        }

        if (string.IsNullOrWhiteSpace(
                request.ChiefComplaint) &&
            string.IsNullOrWhiteSpace(
                request.ClinicalNotes) &&
            string.IsNullOrWhiteSpace(
                request.ExaminationNotes) &&
            string.IsNullOrWhiteSpace(
                request.TreatmentNotes) &&
            string.IsNullOrWhiteSpace(
                request.Advice))
        {
            return ApiResponse<ConsultationResponse>
                .FailureResponse(
                    "At least one consultation detail is required.");
        }

        consultation.ChiefComplaint =
            CleanText(request.ChiefComplaint);

        consultation.ClinicalNotes =
            CleanText(request.ClinicalNotes);

        consultation.ExaminationNotes =
            CleanText(request.ExaminationNotes);

        consultation.TreatmentNotes =
            CleanText(request.TreatmentNotes);

        consultation.Advice =
            CleanText(request.Advice);

        consultation.UpdatedAt =
            DateTime.UtcNow;

        await _repository.UpdateAsync(
            consultation);

        var response =
            await _repository.GetByAppointmentIdAsync(
                hospitalId,
                doctor.Id,
                consultation.AppointmentId);

        if (response == null)
        {
            return ApiResponse<ConsultationResponse>
                .FailureResponse(
                    "Consultation updated but could not be retrieved.");
        }

        return ApiResponse<ConsultationResponse>
            .SuccessResponse(
                response,
                "Consultation updated successfully.");
    }

    // =========================================================
    // COMPLETE CONSULTATION
    // =========================================================

    public async Task<ApiResponse<string>>
        CompleteAsync(
            Guid consultationId)
    {
        var hospitalId =
            _currentUserService.HospitalId;

        var userId =
            _currentUserService.UserId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Hospital context not found.");
        }

        if (userId == Guid.Empty)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "User context not found.");
        }

        if (consultationId == Guid.Empty)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Consultation ID is required.");
        }

        var doctor =
            await GetDoctorAsync(
                hospitalId,
                userId);

        if (doctor == null)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Doctor profile not found.");
        }

        var consultation =
            await _repository.GetEntityByIdAsync(
                hospitalId,
                doctor.Id,
                consultationId);

        if (consultation == null)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Consultation not found.");
        }

        if (consultation.IsCompleted)
        {
            return ApiResponse<string>
                .FailureResponse(
                    "Consultation is already completed.");
        }

        await _repository.CompleteAsync(
            hospitalId,
            doctor.Id,
            consultationId);

        return ApiResponse<string>
            .SuccessResponse(
                "Consultation completed successfully.",
                "Success");
    }

    // =========================================================
    // GET DOCTOR
    // =========================================================

    private async Task<Doctor?>
        GetDoctorAsync(
            Guid hospitalId,
            Guid userId)
    {
        // This method will use the existing doctor repository.
        return await _doctorRepository.GetByUserIdAsync(
            hospitalId,
            userId);
    }


    // =========================================================
    // CLEAN TEXT
    // =========================================================

    private static string? CleanText(
        string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}