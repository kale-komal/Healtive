using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Doctor;
using Healtive.Core.Entities;

namespace Healtive.Application.Interfaces;

public interface IDoctorRepository
{
    Task<bool> ExistsByCodeAsync(
        Guid hospitalId,
        string doctorCode);

    Task<bool> ExistsByRegistrationNumberAsync(
        string registrationNumber);

    Task<bool> ExistsByRegistrationNumberAsync(
        Guid doctorId,
        string registrationNumber);

    Task<bool> ExistsByEmailAsync(
        string email);

    Task<bool> ExistsByMobileAsync(
        string mobileNumber);

    Task CreateAsync(
        Doctor doctor,
        User user,
        Role role,
        UserRole userRole);

    Task UpdateAsync(
        Doctor doctor,
        User user);

    Task<PagedResponse<DoctorListResponse>> GetAllAsync(
        Guid hospitalId,
        DoctorFilterRequest request);

    Task<DoctorResponse?> GetByIdAsync(
        Guid hospitalId,
        Guid doctorId);

    Task<Doctor?> GetEntityByIdAsync(
        Guid hospitalId,
        Guid doctorId);


    Task ResetPasswordAsync(
    Guid hospitalId,
    Guid doctorId,
    string passwordHash);
    // =========================================================
    // DOCTOR PORTAL
    // =========================================================

    Task<Doctor?> GetByUserIdAsync(
        Guid hospitalId,
        Guid userId);

    Task<User?> GetUserByIdAsync(
        Guid userId);

    Task DeleteAsync(
        Guid hospitalId,
        Guid doctorId);

    Task ActivateAsync(
        Guid hospitalId,
        Guid doctorId);

    Task DeactivateAsync(
        Guid hospitalId,
        Guid doctorId);

    Task<Role?> GetRoleByNameAsync(
        Guid hospitalId,
        string roleName);
}