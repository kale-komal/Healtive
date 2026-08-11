using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Subscription;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;

namespace Healtive.Infrastructure.Services.HospitalSubscriptions;

public class HospitalSubscriptionService : IHospitalSubscriptionService
{
    private readonly IHospitalSubscriptionRepository _repository;

    public HospitalSubscriptionService(
        IHospitalSubscriptionRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<IEnumerable<HospitalSubscriptionListResponse>>> GetAllAsync()
    {
        var data = await _repository.GetAllAsync();

        return ApiResponse<IEnumerable<HospitalSubscriptionListResponse>>
            .SuccessResponse(
                data,
                "Hospital subscriptions fetched successfully.");
    }

    public async Task<ApiResponse<HospitalSubscriptionResponse>> GetByIdAsync(Guid id)
    {
        var subscription = await _repository.GetByIdAsync(id);

        if (subscription == null)
        {
            return ApiResponse<HospitalSubscriptionResponse>
                .FailureResponse("Subscription not found.");
        }

        var response = new HospitalSubscriptionResponse
        {
            Id = subscription.Id,
            HospitalId = subscription.HospitalId,
            SubscriptionPlanId = subscription.SubscriptionPlanId,
            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate,
            TrialEndsOn = subscription.TrialEndsOn,
            AmountPaid = subscription.AmountPaid,
            PaymentStatus = subscription.PaymentStatus,
            IsActive = subscription.IsActive
        };

        return ApiResponse<HospitalSubscriptionResponse>
            .SuccessResponse(
                response,
                "Subscription fetched successfully.");
    }

    public async Task<ApiResponse<HospitalSubscriptionResponse>> CreateAsync(
        CreateHospitalSubscriptionRequest request)
    {

        if (await _repository.HasActiveSubscriptionAsync(request.HospitalId))
        {
            return ApiResponse<HospitalSubscriptionResponse>
                .FailureResponse(
                    "This hospital already has an active subscription.");
        }

        var subscription = new HospitalSubscription
        {
            Id = Guid.NewGuid(),
            HospitalId = request.HospitalId,
            SubscriptionPlanId = request.SubscriptionPlanId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            TrialEndsOn = request.TrialEndsOn,
            AmountPaid = request.AmountPaid,
            PaymentStatus = request.PaymentStatus,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.CreateAsync(subscription);

        var response = new HospitalSubscriptionResponse
        {
            Id = subscription.Id,
            HospitalId = subscription.HospitalId,
            SubscriptionPlanId = subscription.SubscriptionPlanId,
            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate,
            TrialEndsOn = subscription.TrialEndsOn,
            AmountPaid = subscription.AmountPaid,
            PaymentStatus = subscription.PaymentStatus,
            IsActive = subscription.IsActive
        };

        return ApiResponse<HospitalSubscriptionResponse>
            .SuccessResponse(
                response,
                "Hospital subscription created successfully.");
    }

    public async Task<ApiResponse<string>> UpdateAsync(
        Guid id,
        UpdateHospitalSubscriptionRequest request)
    {
        var subscription = await _repository.GetByIdAsync(id);

        if (subscription == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Subscription not found.");
        }

        subscription.SubscriptionPlanId = request.SubscriptionPlanId;
        subscription.StartDate = request.StartDate;
        subscription.EndDate = request.EndDate;
        subscription.TrialEndsOn = request.TrialEndsOn;
        subscription.AmountPaid = request.AmountPaid;
        subscription.PaymentStatus = request.PaymentStatus;
        subscription.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(subscription);

        return ApiResponse<string>.SuccessResponse(
            "Subscription updated successfully.",
            "Success");
    }

    public async Task<ApiResponse<string>> DeleteAsync(Guid id)
    {
        var subscription = await _repository.GetByIdAsync(id);

        if (subscription == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Subscription not found.");
        }

        await _repository.DeleteAsync(id);

        return ApiResponse<string>.SuccessResponse(
            "Subscription deleted successfully.",
            "Success");
    }

    public async Task<ApiResponse<string>> RenewAsync(Guid id)
    {
        var subscription = await _repository.GetByIdAsync(id);

        if (subscription == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Subscription not found.");
        }

        subscription.StartDate = DateTime.UtcNow;
        subscription.EndDate = subscription.EndDate.AddDays(30);
        subscription.IsActive = true;
        subscription.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(subscription);

        return ApiResponse<string>.SuccessResponse(
            "Subscription renewed successfully.",
            "Success");
    }

    public async Task<ApiResponse<string>> CancelAsync(Guid id)
    {
        var subscription = await _repository.GetByIdAsync(id);

        if (subscription == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Subscription not found.");
        }

        subscription.IsActive = false;
        subscription.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(subscription);

        return ApiResponse<string>.SuccessResponse(
            "Subscription cancelled successfully.",
            "Success");
    }
}