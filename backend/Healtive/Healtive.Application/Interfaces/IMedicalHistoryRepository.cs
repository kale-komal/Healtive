using Healtive.Application.DTOs.Doctor.MedicalHistory;

namespace Healtive.Application.Interfaces.Repositories;

public interface IMedicalHistoryRepository
{
    Task<IEnumerable<MedicalHistoryResponse>> GetByPatientIdAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid patientId);

    Task<MedicalHistoryResponse?> GetByIdAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid historyId);

    Task<MedicalHistoryResponse> CreateAsync(
        Guid hospitalId,
        Guid doctorId,
        CreateMedicalHistoryRequest request);

    Task<bool> UpdateAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid historyId,
        UpdateMedicalHistoryRequest request);

    Task<bool> DeleteAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid historyId);
}