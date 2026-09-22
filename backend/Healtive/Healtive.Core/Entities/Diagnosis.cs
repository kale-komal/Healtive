namespace Healtive.Core.Entities;

public class Diagnosis
{
    public Guid Id { get; set; }

    public Guid AppointmentId { get; set; }

    public Guid PatientId { get; set; }

    public Guid HospitalId { get; set; }

    public Guid DoctorId { get; set; }

    public string DiagnosisName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Severity { get; set; }

    public string? Notes { get; set; }

    public DateTime DiagnosedAt { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}