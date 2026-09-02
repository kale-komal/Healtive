using Healtive.Application.DTOs.Doctor.PatientMedicalHistory;

namespace Healtive.Application.Interfaces;

public interface IDoctorPatientMedicalHistoryService
{
    Task<DoctorPatientMedicalHistoryPageResponse?>
        GetMedicalHistoryAsync(
            Guid patientId,
            DoctorPatientMedicalHistoryFilterRequest request);
}