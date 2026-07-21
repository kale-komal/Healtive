namespace Healtive.Core.Entities;

public class PrescriptionTemplate
{
    public Guid Id { get; set; }

    public Guid HospitalId { get; set; }

    public Guid DoctorId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Diagnosis { get; set; }

    public string? Advice { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}