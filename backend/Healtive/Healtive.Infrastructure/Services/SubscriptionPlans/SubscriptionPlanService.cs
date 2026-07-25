using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.SubscriptionPlan;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;

namespace Healtive.Infrastructure.Services.SubscriptionPlans;

public class SubscriptionPlanService : ISubscriptionPlanService
{
    private readonly ISubscriptionPlanRepository _repository;

    public SubscriptionPlanService(
        ISubscriptionPlanRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<IEnumerable<SubscriptionPlanListResponse>>> GetAllAsync()
    {
        var plans = await _repository.GetAllAsync();

        return ApiResponse<IEnumerable<SubscriptionPlanListResponse>>
            .SuccessResponse(
                plans,
                "Subscription plans fetched successfully.");
    }

    public async Task<ApiResponse<SubscriptionPlanResponse>> GetByIdAsync(Guid id)
    {
        var plan = await _repository.GetByIdAsync(id);

        if (plan == null)
        {
            return ApiResponse<SubscriptionPlanResponse>
                .FailureResponse("Subscription plan not found.");
        }

        var response = new SubscriptionPlanResponse
        {
            Id = plan.Id,
            Name = plan.Name,
            Description = plan.Description,
            Price = plan.Price,
            DurationInDays = plan.DurationInDays,
            MaxBranches = plan.MaxBranches,
            MaxDoctors = plan.MaxDoctors,
            MaxPatients = plan.MaxPatients,
            IsTrial = plan.IsTrial,
            IsActive = plan.IsActive
        };

        return ApiResponse<SubscriptionPlanResponse>
            .SuccessResponse(
                response,
                "Subscription plan fetched successfully.");
    }

    public async Task<ApiResponse<SubscriptionPlanResponse>> CreateAsync(
        CreateSubscriptionPlanRequest request)
    {
        if (await _repository.ExistsByNameAsync(request.Name))
        {
            return ApiResponse<SubscriptionPlanResponse>
                .FailureResponse("Subscription plan already exists.");
        }

        var plan = new SubscriptionPlan
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            DurationInDays = request.DurationInDays,
            MaxBranches = request.MaxBranches,
            MaxDoctors = request.MaxDoctors,
            MaxPatients = request.MaxPatients,
            IsTrial = request.IsTrial,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.CreateAsync(plan);

        var response = new SubscriptionPlanResponse
        {
            Id = plan.Id,
            Name = plan.Name,
            Description = plan.Description,
            Price = plan.Price,
            DurationInDays = plan.DurationInDays,
            MaxBranches = plan.MaxBranches,
            MaxDoctors = plan.MaxDoctors,
            MaxPatients = plan.MaxPatients,
            IsTrial = plan.IsTrial,
            IsActive = plan.IsActive
        };

        return ApiResponse<SubscriptionPlanResponse>
            .SuccessResponse(
                response,
                "Subscription plan created successfully.");
    }

    public async Task<ApiResponse<string>> UpdateAsync(
        Guid id,
        UpdateSubscriptionPlanRequest request)
    {
        var plan = await _repository.GetByIdAsync(id);

        if (plan == null)
        {
            return ApiResponse<string>
                .FailureResponse("Subscription plan not found.");
        }

        if (await _repository.ExistsByNameAsync(id, request.Name))
        {
            return ApiResponse<string>
                .FailureResponse("Subscription plan already exists.");
        }

        plan.Name = request.Name;
        plan.Description = request.Description;
        plan.Price = request.Price;
        plan.DurationInDays = request.DurationInDays;
        plan.MaxBranches = request.MaxBranches;
        plan.MaxDoctors = request.MaxDoctors;
        plan.MaxPatients = request.MaxPatients;
        plan.IsTrial = request.IsTrial;
        plan.IsActive = request.IsActive;
        plan.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(plan);

        return ApiResponse<string>
            .SuccessResponse(
                "Subscription plan updated successfully.",
                "Success");
    }

    public async Task<ApiResponse<string>> DeleteAsync(Guid id)
    {
        var plan = await _repository.GetByIdAsync(id);

        if (plan == null)
        {
            return ApiResponse<string>
                .FailureResponse("Subscription plan not found.");
        }

        await _repository.DeleteAsync(id);

        return ApiResponse<string>
            .SuccessResponse(
                "Subscription plan deleted successfully.",
                "Success");
    }
}