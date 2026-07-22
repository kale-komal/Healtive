namespace Healtive.Core.Entities;

public class MedicineStock
{
    public Guid Id { get; set; }

    public Guid MedicineId { get; set; }

    public Guid BranchId { get; set; }

    public string? BatchNumber { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public decimal Quantity { get; set; }

    public decimal? PurchasePrice { get; set; }

    public decimal? SellingPrice { get; set; }

    public DateTime CreatedAt { get; set; }
}