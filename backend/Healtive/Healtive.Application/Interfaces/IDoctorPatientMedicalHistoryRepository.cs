using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Doctor.PatientMedicalHistory;

namespace Healtive.Application.Interfaces;

public interface IDoctorPatientMedicalHistoryRepository
{
    // =========================================================
    // PATIENT MEDICAL HISTORY
    // =========================================================

    Task<DoctorPatientMedicalHistorySummaryResponse?>
        GetSummaryAsync(
            Guid hospitalId,
            Guid patientId);

    Task<PagedResponse<DoctorPatientMedicalHistoryResponse>>
        GetHistoryAsync(
            Guid hospitalId,
            Guid patientId,
            DoctorPatientMedicalHistoryFilterRequest request);
    Task<bool> IsPatientAssociatedWithDoctorAsync(
    Guid hospitalId,
    Guid doctorId,
    Guid patientId);
}