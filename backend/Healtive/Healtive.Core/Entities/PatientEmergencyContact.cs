namespace Healtive.Core.Entities;

public class PatientEmergencyContact
{
    public Guid Id { get; set; }

    public Guid PatientId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Relationship { get; set; } = string.Empty;

    public string MobileNumber { get; set; } = string.Empty;

    public string? AlternateNumber { get; set; }

    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; }
}