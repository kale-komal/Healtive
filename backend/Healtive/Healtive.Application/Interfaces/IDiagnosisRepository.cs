using Healtive.Application.DTOs.Doctor.Diagnosis;

namespace Healtive.Application.Interfaces.Repositories;

public interface IDiagnosisRepository
{
    Task<IEnumerable<DiagnosisResponse>> GetByAppointmentIdAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid appointmentId);

    Task<DiagnosisResponse?> GetByIdAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid diagnosisId);

    Task<DiagnosisResponse> CreateAsync(
        Guid hospitalId,
        Guid doctorId,
        CreateDiagnosisRequest request);

    Task<bool> UpdateAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid diagnosisId,
        UpdateDiagnosisRequest request);

    Task<bool> DeleteAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid diagnosisId);
}