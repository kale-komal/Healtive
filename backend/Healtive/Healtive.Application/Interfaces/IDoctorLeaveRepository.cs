using Healtive.Application.DTOs.DoctorLeave;
using Healtive.Core.Entities;

namespace Healtive.Application.Interfaces;

public interface IDoctorLeaveRepository
{
    Task<Doctor?> GetDoctorAsync(
        Guid hospitalId,
        Guid doctorId);

    Task<DoctorLeave?> GetByIdAsync(
        Guid doctorId,
        Guid id);

    Task<bool> HasOverlapAsync(
        Guid doctorId,
        DateOnly fromDate,
        DateOnly toDate,
        Guid? excludeId = null);

    Task CreateAsync(
        DoctorLeave leave);

    Task UpdateAsync(
        DoctorLeave leave);

    Task<IEnumerable<DoctorLeaveResponse>>
        GetByDoctorAsync(
            Guid hospitalId,
            Guid doctorId);

    Task DeleteAsync(
        Guid doctorId,
        Guid id);

    Task ApproveAsync(
        Guid doctorId,
        Guid id);

    Task RejectAsync(
        Guid doctorId,
        Guid id);
}