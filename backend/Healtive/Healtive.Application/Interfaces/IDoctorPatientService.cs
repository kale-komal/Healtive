using Healtive.Application.DTOs.Doctor.PatientProfile;

namespace Healtive.Application.Interfaces;

public interface IDoctorPatientService
{
    // =========================================================
    // GET PATIENT PROFILE
    // =========================================================

    Task<DoctorPatientProfileResponse?>
        GetPatientProfileAsync(
            Guid patientId);
}