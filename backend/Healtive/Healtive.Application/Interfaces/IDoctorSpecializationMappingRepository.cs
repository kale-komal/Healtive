using Healtive.Application.DTOs.Doctor;
using Healtive.Application.DTOs.DoctorSpecialization;
using Healtive.Core.Entities;

namespace Healtive.Application.Interfaces;

public interface IDoctorSpecializationMappingRepository
{
    Task<Doctor?> GetDoctorAsync(
        Guid hospitalId,
        Guid doctorId);

    Task<DoctorSpecialization?> GetSpecializationAsync(
        Guid specializationId);

    Task<bool> MappingExistsAsync(
        Guid doctorId,
        Guid specializationId);

    Task AssignAsync(
        DoctorSpecializationMapping mapping);

    Task<IEnumerable<DoctorSpecializationMappingResponse>>
        GetDoctorSpecializationsAsync(
            Guid hospitalId,
            Guid doctorId);

    Task RemoveAsync(
        Guid doctorId,
        Guid specializationId);

    Task<IEnumerable<DoctorListResponse>>
        GetSpecializationDoctorsAsync(
            Guid hospitalId,
            Guid specializationId);
}