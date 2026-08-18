using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Staff;
using Healtive.Core.Entities;

namespace Healtive.Application.Interfaces;

public interface IStaffRepository
{
    Task<bool> UsernameExistsAsync(
        Guid hospitalId,
        string username);

    Task<bool> EmployeeCodeExistsAsync(
        Guid hospitalId,
        string employeeCode);

    Task<bool> EmailExistsAsync(
        Guid hospitalId,
        string email);

    Task<bool> MobileNumberExistsAsync(
        Guid hospitalId,
        string mobileNumber);

    Task<bool> UsernameExistsAsync(
        Guid hospitalId,
        Guid userId,
        string username);

    Task<bool> EmployeeCodeExistsAsync(
        Guid hospitalId,
        Guid userId,
        string employeeCode);

    Task<bool> EmailExistsAsync(
        Guid hospitalId,
        Guid userId,
        string email);

    Task<bool> MobileNumberExistsAsync(
        Guid hospitalId,
        Guid userId,
        string mobileNumber);

    Task<bool> RoleExistsAsync(
        Guid hospitalId,
        Guid roleId);

    Task CreateAsync(User user);

    Task AssignRoleAsync(UserRole userRole);

    Task UpdateAsync(User user);

    Task UpdateRoleAsync(
        Guid userId,
        Guid roleId);

    Task<PagedResponse<StaffListResponse>> GetAllAsync(
        StaffFilterRequest request,
        Guid hospitalId);

    Task<StaffResponse?> GetByIdAsync(
        Guid hospitalId,
        Guid userId);

    Task<User?> GetUserByIdAsync(
        Guid hospitalId,
        Guid userId);

    Task DeleteAsync(
        Guid hospitalId,
        Guid userId);

    Task ActivateAsync(
        Guid hospitalId,
        Guid userId);

    Task DeactivateAsync(
        Guid hospitalId,
        Guid userId);
}