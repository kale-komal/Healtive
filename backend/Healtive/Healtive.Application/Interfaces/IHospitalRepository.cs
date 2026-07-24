using Healtive.Application.DTOs.Hospital;
using Healtive.Core.Entities;

namespace Healtive.Application.Interfaces;

public interface IHospitalRepository
{
   

    Task CreateAsync(Hospital hospital);

    Task<IEnumerable<HospitalListResponse>> GetAllAsync();

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
}