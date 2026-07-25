namespace Healtive.Application.DTOs.Subscription;

public class HospitalSubscriptionListResponse
{
    public Guid Id { get; set; }

    public string HospitalName { get; set; } = string.Empty;

    public string PlanName { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal AmountPaid { get; set; }

    public string PaymentStatus { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}