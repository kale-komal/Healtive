using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Patient;

namespace Healtive.Application.Interfaces;

public interface IPatientService
{
    Task<PatientResponse> CreateAsync(
        CreatePatientRequest request);

    Task<PatientResponse> UpdateAsync(
        Guid id,
        UpdatePatientRequest request);

    Task<PagedResponse<PatientResponse>> GetAllAsync(
        PatientFilterRequest request);

    Task<PatientResponse> GetByIdAsync(
        Guid id);

    Task<bool> DeleteAsync(
        Guid id);

    Task<bool> ActivateAsync(
        Guid id);

    Task<bool> DeactivateAsync(
        Guid id);
}