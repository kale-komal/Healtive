namespace Healtive.Application.DTOs.Doctor.Prescription;

public class UpdatePrescriptionRequest
{
    public string? Diagnosis { get; set; }

    public string? ClinicalNotes { get; set; }

    public string? Advice { get; set; }

    public DateOnly? FollowUpDate { get; set; }

    public bool IsFinalized { get; set; }

    public List<PrescriptionItemRequest> Items { get; set; } = new();
}