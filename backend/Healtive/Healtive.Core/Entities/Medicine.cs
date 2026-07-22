namespace Healtive.Core.Entities;

public class Medicine
{
    public Guid Id { get; set; }

    public string MedicineCode { get; set; } = string.Empty;

    public Guid CategoryId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? GenericName { get; set; }

    public string? BrandName { get; set; }

    public string? Strength { get; set; }

    public string? Manufacturer { get; set; }

    public string? Unit { get; set; }

    public decimal? MRP { get; set; }

    public decimal? SellingPrice { get; set; }

    public bool IsPrescriptionRequired { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}