namespace Healtive.Core.Entities;

public class MedicineSale
{
    public Guid Id { get; set; }

    public Guid BillId { get; set; }

    public Guid MedicineId { get; set; }

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime SoldAt { get; set; }
}