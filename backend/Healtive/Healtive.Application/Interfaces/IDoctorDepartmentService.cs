using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Doctor;

namespace Healtive.Application.Interfaces;

public interface IDoctorDepartmentService
{
    Task<ApiResponse<string>> AssignAsync(
        Guid doctorId,
        AssignDoctorDepartmentRequest request);

    Task<ApiResponse<IEnumerable<DoctorDepartmentResponse>>>
        GetDoctorDepartmentsAsync(
            Guid doctorId);

    Task<ApiResponse<string>> RemoveAsync(
        Guid doctorId,
        Guid departmentId);

    Task<ApiResponse<IEnumerable<DoctorListResponse>>>
        GetDepartmentDoctorsAsync(
            Guid departmentId);
}