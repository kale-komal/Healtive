namespace Healtive.Application.DTOs.Doctor.Lab;

public class LabTestResponse
{
    public Guid Id { get; set; }

    public string TestCode { get; set; } = string.Empty;

    public Guid CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? NormalRange { get; set; }

    public string? Unit { get; set; }
}