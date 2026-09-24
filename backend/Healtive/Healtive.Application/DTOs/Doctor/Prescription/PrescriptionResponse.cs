namespace Healtive.Application.DTOs.Doctor.Prescription;

public class PrescriptionResponse
{
    public Guid Id { get; set; }

    public string PrescriptionNumber { get; set; } = string.Empty;

    public Guid HospitalId { get; set; }

    public Guid BranchId { get; set; }

    public Guid AppointmentId { get; set; }

    public Guid PatientId { get; set; }

    public Guid DoctorId { get; set; }

    public DateTime PrescriptionDate { get; set; }

    public string? Diagnosis { get; set; }

    public string? ClinicalNotes { get; set; }

    public string? Advice { get; set; }

    public DateOnly? FollowUpDate { get; set; }

    public bool IsFinalized { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public List<PrescriptionItemResponse> Items { get; set; } = new();
}