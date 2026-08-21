using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.DoctorSpecialization;
using Healtive.Core.Entities;

namespace Healtive.Application.Interfaces;

public interface IDoctorSpecializationRepository
{
    Task<bool> ExistsByNameAsync(string name);

    Task<bool> ExistsByNameAsync(
        Guid id,
        string name);

    Task<bool> ExistsByCodeAsync(string code);

    Task<bool> ExistsByCodeAsync(
        Guid id,
        string code);

    Task CreateAsync(
        DoctorSpecialization specialization);

    Task UpdateAsync(
        DoctorSpecialization specialization);

    Task<PagedResponse<DoctorSpecializationResponse>> GetAllAsync(
        DoctorSpecializationFilterRequest request);

    Task<DoctorSpecializationResponse?> GetByIdAsync(
        Guid id);

    Task<DoctorSpecialization?> GetEntityByIdAsync(
        Guid id);

    Task DeleteAsync(
        Guid id);

    Task ActivateAsync(
        Guid id);

    Task DeactivateAsync(
        Guid id);
}