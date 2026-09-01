using Healtive.Application.DTOs.Doctor.PatientProfile;
using Healtive.Core.Entities;

namespace Healtive.Application.Interfaces;

public interface IDoctorPatientRepository
{
    // =========================================================
    // GET PATIENT PROFILE
    // =========================================================

    Task<DoctorPatientProfileResponse?>
        GetPatientProfileAsync(
            Guid hospitalId,
            Guid doctorId,
            Guid patientId);

    // =========================================================
    // GET PATIENT ADDRESS
    // =========================================================

    Task<DoctorPatientAddressResponse?>
        GetPatientAddressAsync(
            Guid patientId);

    // =========================================================
    // GET EMERGENCY CONTACT
    // =========================================================

    Task<DoctorPatientEmergencyContactResponse?>
        GetEmergencyContactAsync(
            Guid patientId);

    // =========================================================
    // GET INSURANCE
    // =========================================================

    Task<DoctorPatientInsuranceResponse?>
        GetInsuranceAsync(
            Guid patientId);

    // =========================================================
    // VERIFY PATIENT IS RELATED TO DOCTOR
    // =========================================================

    Task<bool> IsPatientAssociatedWithDoctorAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid patientId);
}