using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Subscription;

namespace Healtive.Application.Interfaces;

public interface IHospitalSubscriptionService
{
    Task<ApiResponse<IEnumerable<HospitalSubscriptionListResponse>>> GetAllAsync();

    Task<ApiResponse<HospitalSubscriptionResponse>> GetByIdAsync(Guid id);

    Task<ApiResponse<HospitalSubscriptionResponse>> CreateAsync(CreateHospitalSubscriptionRequest request);

    Task<ApiResponse<string>> UpdateAsync(
        Guid id,
        UpdateHospitalSubscriptionRequest request);

    Task<ApiResponse<string>> DeleteAsync(Guid id);

    Task<ApiResponse<string>> RenewAsync(Guid id);

    Task<ApiResponse<string>> CancelAsync(Guid id);
}