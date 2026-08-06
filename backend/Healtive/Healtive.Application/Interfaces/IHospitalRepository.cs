using Healtive.Application.DTOs.Hospital;
using Healtive.Core.Entities;
using Healtive.Application.DTOs.Common;


namespace Healtive.Application.Interfaces;

public interface IHospitalRepository
{
   

    Task CreateAsync(Hospital hospital);

    Task<PagedResponse<HospitalListResponse>> GetAllAsync(
    HospitalFilterRequest request);
    Task<Hospital?> GetByIdAsync(Guid id);

    Task UpdateAsync(Hospital hospital);

    Task DeleteAsync(Guid id);
    Task CreateRoleAsync(Role role);

    Task CreateUserAsync(User user);

    Task AssignRoleAsync(UserRole userRole);

    Task<bool> UsernameExistsAsync(string username);
    Task<bool> ExistsByCodeAsync(string code);
    Task<bool> ExistsByEmailAsync(string email);
    Task<bool> ExistsByMobileAsync(string phoneNumber);

    Task<bool> ExistsByCodeAsync(Guid id, string code);
    Task<bool> ExistsByEmailAsync(Guid id, string email);
    Task<bool> ExistsByMobileAsync(Guid id, string phoneNumber);


    Task ActivateAsync(Guid id);

    Task DeactivateAsync(Guid id);
    Task<string?> GetLastHospitalCodeAsync();
}