namespace Healtive.Application.DTOs.Doctor.MedicalHistory;

public class UpdateMedicalHistoryRequest
{
    public string? MedicalCondition { get; set; }

    public string? Diagnosis { get; set; }

    public string? Treatment { get; set; }

    public string? Notes { get; set; }
}