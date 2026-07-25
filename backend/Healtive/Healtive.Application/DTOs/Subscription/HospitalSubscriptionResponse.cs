namespace Healtive.Application.DTOs.Subscription;

public class HospitalSubscriptionResponse
{
    public Guid Id { get; set; }

    public Guid HospitalId { get; set; }

    public string HospitalName { get; set; } = string.Empty;

    public Guid SubscriptionPlanId { get; set; }

    public string PlanName { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime? TrialEndsOn { get; set; }

    public decimal AmountPaid { get; set; }

    public string PaymentStatus { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}