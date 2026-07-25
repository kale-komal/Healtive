namespace Healtive.Application.DTOs.SubscriptionPlan;

public class CreateSubscriptionPlanRequest
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int DurationInDays { get; set; }

    public int MaxBranches { get; set; }

    public int MaxDoctors { get; set; }

    public int MaxPatients { get; set; }

    public bool IsTrial { get; set; }

    public bool IsActive { get; set; } = true;
}