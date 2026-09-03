namespace Healtive.Application.DTOs.Doctor.Consultation;

public class CreateConsultationRequest
{
    public Guid AppointmentId { get; set; }

    public string? ChiefComplaint { get; set; }

    public string? ClinicalNotes { get; set; }

    public string? ExaminationNotes { get; set; }

    public string? TreatmentNotes { get; set; }

    public string? Advice { get; set; }
}