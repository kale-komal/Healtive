using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Patient;
using Healtive.Core.Entities;

namespace Healtive.Application.Interfaces;

public interface IPatientRepository
{
    Task<bool> ExistsByMobileAsync(string mobileNumber);

    Task<bool> ExistsByMobileAsync(
        Guid id,
        string mobileNumber);

    Task<bool> ExistsByEmailAsync(string email);

    Task<bool> ExistsByEmailAsync(
        Guid id,
        string email);

    Task CreateAsync(Patient patient);

    Task UpdateAsync(Patient patient);

    Task<PagedResponse<PatientResponse>> GetAllAsync(
        PatientFilterRequest request);

    Task<PatientResponse?> GetByIdAsync(Guid id);

    Task<Patient?> GetEntityByIdAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task ActivateAsync(Guid id);

    Task DeactivateAsync(Guid id);
}