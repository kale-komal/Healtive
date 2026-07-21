namespace Healtive.Core.Entities;

public class Bill
{
    public Guid Id { get; set; }

    public string BillNumber { get; set; } = string.Empty;

    public Guid HospitalId { get; set; }

    public Guid BranchId { get; set; }

    public Guid PatientId { get; set; }

    public Guid? AppointmentId { get; set; }

    public DateTime BillDate { get; set; }

    public decimal SubTotal { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal PaidAmount { get; set; }

    public decimal BalanceAmount { get; set; }

    public string BillStatus { get; set; } = string.Empty;

    public string? Remarks { get; set; }

    public Guid CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}