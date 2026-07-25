namespace Healtive.Application.DTOs.Subscription;

public class RenewSubscriptionRequest
{
    public DateTime NewEndDate { get; set; }

    public decimal AmountPaid { get; set; }

    public string PaymentStatus { get; set; } = string.Empty;
}