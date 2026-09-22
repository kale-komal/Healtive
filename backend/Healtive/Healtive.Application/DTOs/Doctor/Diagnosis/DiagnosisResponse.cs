namespace Healtive.Application.DTOs.Doctor.Diagnosis;

public class DiagnosisResponse
{
    public Guid Id { get; set; }

    public Guid AppointmentId { get; set; }

    public Guid PatientId { get; set; }

    public string PatientName { get; set; } = string.Empty;

    public Guid DoctorId { get; set; }

    public string DoctorName { get; set; } = string.Empty;

    public string DiagnosisName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Severity { get; set; }

    public string? Notes { get; set; }

    public DateTime DiagnosedAt { get; set; }

    public bool IsActive { get; set; }
}