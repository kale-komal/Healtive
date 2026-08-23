using Healtive.Application.DTOs.DoctorAvailability;
using Healtive.Core.Entities;

namespace Healtive.Application.Interfaces;

public interface IDoctorAvailabilityRepository
{
    Task<Doctor?> GetDoctorAsync(
        Guid hospitalId,
        Guid doctorId);

    Task<DoctorAvailability?> GetByIdAsync(
        Guid doctorId,
        Guid id);

    Task<bool> HasOverlapAsync(
        Guid doctorId,
        byte dayOfWeek,
        TimeSpan startTime,
        TimeSpan endTime,
        Guid? excludeId = null);

    Task CreateAsync(
        DoctorAvailability availability);

    Task UpdateAsync(
        DoctorAvailability availability);

    Task<IEnumerable<DoctorAvailabilityResponse>>
        GetByDoctorAsync(
            Guid hospitalId,
            Guid doctorId);

    Task DeleteAsync(
        Guid doctorId,
        Guid id);

    Task ActivateAsync(
        Guid doctorId,
        Guid id);

    Task DeactivateAsync(
        Guid doctorId,
        Guid id);
}