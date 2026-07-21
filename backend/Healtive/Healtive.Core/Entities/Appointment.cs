namespace Healtive.Core.Entities;

public class Appointment
{
    public Guid Id { get; set; }

    public string AppointmentNumber { get; set; } = string.Empty;

    public Guid HospitalId { get; set; }

    public Guid BranchId { get; set; }

    public Guid PatientId { get; set; }

    public Guid DoctorId { get; set; }

    public Guid DepartmentId { get; set; }

    public Guid AppointmentStatusId { get; set; }

    public DateOnly AppointmentDate { get; set; }

    public TimeSpan AppointmentTime { get; set; }

    public int? TokenNumber { get; set; }

    public string ConsultationType { get; set; } = string.Empty;

    public string? ReasonForVisit { get; set; }

    public string? Notes { get; set; }

    public bool IsFirstVisit { get; set; }

    public Guid CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}