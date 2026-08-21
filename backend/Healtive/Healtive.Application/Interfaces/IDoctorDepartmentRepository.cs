using Healtive.Application.DTOs.Doctor;
using Healtive.Core.Entities;

namespace Healtive.Application.Interfaces;

public interface IDoctorDepartmentRepository
{
    Task<Doctor?> GetDoctorAsync(
        Guid hospitalId,
        Guid doctorId);

    Task<Department?> GetDepartmentAsync(
        Guid hospitalId,
        Guid departmentId);

    Task<bool> MappingExistsAsync(
        Guid doctorId,
        Guid departmentId);

    Task AssignAsync(
        DoctorDepartment mapping);

    Task<IEnumerable<DoctorDepartmentResponse>>
        GetDoctorDepartmentsAsync(
            Guid hospitalId,
            Guid doctorId);

    Task RemoveAsync(
        Guid doctorId,
        Guid departmentId);

    Task<IEnumerable<DoctorListResponse>>
        GetDepartmentDoctorsAsync(
            Guid hospitalId,
            Guid departmentId);
}