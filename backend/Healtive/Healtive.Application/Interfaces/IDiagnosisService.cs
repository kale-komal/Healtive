using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Doctor.Diagnosis;

namespace Healtive.Application.Interfaces;

public interface IDiagnosisService
{
    Task<ApiResponse<IEnumerable<DiagnosisResponse>>> GetByAppointmentIdAsync(
        Guid appointmentId);

    Task<ApiResponse<DiagnosisResponse>> GetByIdAsync(
        Guid diagnosisId);

    Task<ApiResponse<DiagnosisResponse>> CreateAsync(
        CreateDiagnosisRequest request);

    Task<ApiResponse<DiagnosisResponse>> UpdateAsync(
        Guid diagnosisId,
        UpdateDiagnosisRequest request);

    Task<ApiResponse<string>> DeleteAsync(
        Guid diagnosisId);
}