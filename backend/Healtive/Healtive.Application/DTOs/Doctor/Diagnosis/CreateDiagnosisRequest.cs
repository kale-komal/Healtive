namespace Healtive.Application.DTOs.Doctor.Diagnosis;

public class CreateDiagnosisRequest
{
    public Guid AppointmentId { get; set; }

    public string DiagnosisName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Severity { get; set; }

    public string? Notes { get; set; }
}