namespace Healtive.Application.DTOs.Subscription;

public class UpdateHospitalSubscriptionRequest
{
    public Guid SubscriptionPlanId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime? TrialEndsOn { get; set; }

    public decimal AmountPaid { get; set; }

    public string PaymentStatus { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}