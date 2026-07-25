namespace Healtive.Application.DTOs.SubscriptionPlan;

public class SubscriptionPlanListResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int DurationInDays { get; set; }

    public bool IsTrial { get; set; }

    public bool IsActive { get; set; }
}