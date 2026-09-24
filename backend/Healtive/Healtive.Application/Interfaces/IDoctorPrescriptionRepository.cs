using Healtive.Application.DTOs.Doctor.Prescription;

namespace Healtive.Application.Interfaces.Repositories;

public interface IDoctorPrescriptionRepository
{
    Task<IEnumerable<PrescriptionResponse>> GetByAppointmentIdAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid appointmentId);

    Task<PrescriptionResponse?> GetByIdAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid prescriptionId);

    Task<PrescriptionResponse> CreateAsync(
        Guid hospitalId,
        Guid doctorId,
        CreatePrescriptionRequest request);

    Task<bool> UpdateAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid prescriptionId,
        UpdatePrescriptionRequest request);

    Task<bool> DeleteAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid prescriptionId);

    Task<bool> FinalizeAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid prescriptionId);
}