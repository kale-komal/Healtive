using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.SubscriptionPlan;

namespace Healtive.Application.Interfaces;

public interface ISubscriptionPlanService
{
    Task<ApiResponse<IEnumerable<SubscriptionPlanListResponse>>> GetAllAsync();

    Task<ApiResponse<SubscriptionPlanResponse>> GetByIdAsync(Guid id);

    Task<ApiResponse<SubscriptionPlanResponse>> CreateAsync(CreateSubscriptionPlanRequest request);

    Task<ApiResponse<string>> UpdateAsync(
        Guid id,
        UpdateSubscriptionPlanRequest request);

    Task<ApiResponse<string>> DeleteAsync(Guid id);
}