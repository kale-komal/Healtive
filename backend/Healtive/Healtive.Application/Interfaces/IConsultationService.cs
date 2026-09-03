using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Doctor.Consultation;

namespace Healtive.Application.Interfaces;

public interface IConsultationService
{
    // =========================================================
    // CREATE CONSULTATION
    // =========================================================

    Task<ApiResponse<ConsultationResponse>>
        CreateAsync(
            CreateConsultationRequest request);

    // =========================================================
    // GET CONSULTATION BY APPOINTMENT
    // =========================================================

    Task<ApiResponse<ConsultationResponse>>
        GetByAppointmentIdAsync(
            Guid appointmentId);

    // =========================================================
    // UPDATE CONSULTATION
    // =========================================================

    Task<ApiResponse<ConsultationResponse>>
        UpdateAsync(
            Guid consultationId,
            UpdateConsultationRequest request);

    // =========================================================
    // COMPLETE CONSULTATION
    // =========================================================

    Task<ApiResponse<string>>
        CompleteAsync(
            Guid consultationId);
}