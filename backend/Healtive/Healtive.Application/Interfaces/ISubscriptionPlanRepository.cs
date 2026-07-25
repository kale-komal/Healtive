using Healtive.Application.DTOs.SubscriptionPlan;
using Healtive.Core.Entities;

namespace Healtive.Application.Interfaces;

public interface ISubscriptionPlanRepository
{
    Task<bool> ExistsByNameAsync(string name);

    Task<bool> ExistsByNameAsync(Guid id, string name);

    Task CreateAsync(SubscriptionPlan plan);

    Task<IEnumerable<SubscriptionPlanListResponse>> GetAllAsync();

    Task<SubscriptionPlan?> GetByIdAsync(Guid id);

    Task UpdateAsync(SubscriptionPlan plan);

    Task DeleteAsync(Guid id);
}