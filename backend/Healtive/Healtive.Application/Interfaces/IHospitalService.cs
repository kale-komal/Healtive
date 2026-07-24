using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Hospital;

namespace Healtive.Application.Interfaces;

public interface IHospitalService
{
    Task<ApiResponse<HospitalResponse>> CreateAsync(
        CreateHospitalRequest request);

    Task<ApiResponse<IEnumerable<HospitalListResponse>>> GetAllAsync();

    Task<ApiResponse<HospitalResponse>> GetByIdAsync(Guid id);

    Task<ApiResponse<string>> UpdateAsync(
        Guid id,
        UpdateHospitalRequest request);

    Task<ApiResponse<string>> DeleteAsync(Guid id);
}