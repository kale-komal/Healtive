namespace Healtive.Application.DTOs.Doctor.MedicalHistory;

public class MedicalHistoryResponse
{
    public Guid Id { get; set; }

    public Guid PatientId { get; set; }

    public string PatientName { get; set; } = string.Empty;

    public string? MedicalCondition { get; set; }

    public string? Diagnosis { get; set; }

    public string? Treatment { get; set; }

    public string? Notes { get; set; }

    public DateTime? RecordedAt { get; set; }

    public Guid? RecordedByDoctorId { get; set; }

    public string? RecordedByDoctorName { get; set; }
}