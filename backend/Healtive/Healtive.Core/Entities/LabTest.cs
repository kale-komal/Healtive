namespace Healtive.Core.Entities;

public class LabTest
{
    public Guid Id { get; set; }

    public string TestCode { get; set; } = string.Empty;

    public Guid CategoryId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? NormalRange { get; set; }

    public string? Unit { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}