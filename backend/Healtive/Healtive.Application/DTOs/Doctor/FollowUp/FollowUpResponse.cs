namespace Healtive.Application.DTOs.Doctor.FollowUp;

public class FollowUpResponse
{
    public Guid Id { get; set; }

    public Guid HospitalId { get; set; }

    public Guid BranchId { get; set; }

    public Guid AppointmentId { get; set; }

    public Guid PatientId { get; set; }

    public Guid DoctorId { get; set; }

    public DateOnly FollowUpDate { get; set; }

    public string? FollowUpNotes { get; set; }

    public string Status { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }

    public DateTime? CompletedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string PatientName { get; set; } = string.Empty;

    public string DoctorName { get; set; } = string.Empty;
}