namespace Healtive.Core.Entities;

public class Payment
{
    public Guid Id { get; set; }

    public string PaymentNumber { get; set; } = string.Empty;

    public Guid BillId { get; set; }

    public Guid PaymentMethodId { get; set; }

    public decimal Amount { get; set; }

    public string? TransactionReference { get; set; }

    public DateTime PaymentDate { get; set; }

    public Guid ReceivedByUserId { get; set; }

    public string? Remarks { get; set; }

    public DateTime CreatedAt { get; set; }
}