using Healtive.Application.DTOs.Subscription;
using Healtive.Core.Entities;

namespace Healtive.Application.Interfaces;

public interface IHospitalSubscriptionRepository
{
    Task<IEnumerable<HospitalSubscriptionListResponse>> GetAllAsync();

    Task<HospitalSubscription?> GetByIdAsync(Guid id);

    Task CreateAsync(HospitalSubscription subscription);

    Task UpdateAsync(HospitalSubscription subscription);

    Task DeleteAsync(Guid id);
    Task<bool> HasActiveSubscriptionAsync(Guid hospitalId);
}