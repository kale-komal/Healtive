using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Doctor.MedicalHistory;

namespace Healtive.Application.Interfaces;

public interface IMedicalHistoryService
{
    Task<ApiResponse<IEnumerable<MedicalHistoryResponse>>> GetByPatientIdAsync(
        Guid patientId);

    Task<ApiResponse<MedicalHistoryResponse>> GetByIdAsync(
        Guid historyId);

    Task<ApiResponse<MedicalHistoryResponse>> CreateAsync(
        CreateMedicalHistoryRequest request);

    Task<ApiResponse<MedicalHistoryResponse>> UpdateAsync(
        Guid historyId,
        UpdateMedicalHistoryRequest request);

    Task<ApiResponse<string>> DeleteAsync(
        Guid historyId);
}