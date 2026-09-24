namespace Healtive.Application.DTOs.Doctor.Prescription;

public class PrescriptionItemRequest
{
    public string MedicineName { get; set; } = string.Empty;

    public Guid DosageId { get; set; }

    public string? Strength { get; set; }

    public string? Route { get; set; }

    public string? Frequency { get; set; }

    public int DurationDays { get; set; }

    public decimal Quantity { get; set; }

    public string? Instructions { get; set; }
}