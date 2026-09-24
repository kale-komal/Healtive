using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Doctor.Prescription;

namespace Healtive.Application.Interfaces;

public interface IDoctorPrescriptionService
{
    Task<ApiResponse<IEnumerable<PrescriptionResponse>>> GetByAppointmentIdAsync(
        Guid appointmentId);

    Task<ApiResponse<PrescriptionResponse>> GetByIdAsync(
        Guid prescriptionId);

    Task<ApiResponse<PrescriptionResponse>> CreateAsync(
        CreatePrescriptionRequest request);

    Task<ApiResponse<PrescriptionResponse>> UpdateAsync(
        Guid prescriptionId,
        UpdatePrescriptionRequest request);

    Task<ApiResponse<string>> DeleteAsync(
        Guid prescriptionId);

    Task<ApiResponse<string>> FinalizeAsync(
        Guid prescriptionId);
}