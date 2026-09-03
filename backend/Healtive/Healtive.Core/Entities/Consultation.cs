namespace Healtive.Core.Entities;

public class Consultation
{
    public Guid Id { get; set; }

    public Guid HospitalId { get; set; }

    public Guid AppointmentId { get; set; }

    public Guid PatientId { get; set; }

    public Guid DoctorId { get; set; }

    public DateOnly ConsultationDate { get; set; }

    public string? ChiefComplaint { get; set; }

    public string? ClinicalNotes { get; set; }

    public string? ExaminationNotes { get; set; }

    public string? TreatmentNotes { get; set; }

    public string? Advice { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime? CompletedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}