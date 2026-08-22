using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Doctor;
using Healtive.Application.DTOs.DoctorSpecialization;

namespace Healtive.Application.Interfaces;

public interface IDoctorSpecializationMappingService
{
    Task<ApiResponse<bool>> AssignAsync(
        Guid doctorId,
        AssignDoctorSpecializationRequest request);

    Task<ApiResponse<
        IEnumerable<DoctorSpecializationMappingResponse>>>
        GetDoctorSpecializationsAsync(
            Guid doctorId);

    Task<ApiResponse<bool>> RemoveAsync(
        Guid doctorId,
        Guid specializationId);

    Task<ApiResponse<
        IEnumerable<DoctorListResponse>>>
        GetSpecializationDoctorsAsync(
            Guid specializationId);
}