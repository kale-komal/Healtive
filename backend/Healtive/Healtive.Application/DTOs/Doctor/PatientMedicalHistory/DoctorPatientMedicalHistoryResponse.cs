namespace Healtive.Application.DTOs.Doctor.PatientMedicalHistory;

public class DoctorPatientMedicalHistoryResponse
{
    public Guid AppointmentId { get; set; }

    public string AppointmentNumber { get; set; } = string.Empty;

    public DateOnly AppointmentDate { get; set; }

    public TimeSpan AppointmentTime { get; set; }

    public Guid DoctorId { get; set; }

    public string DoctorName { get; set; } = string.Empty;

    public Guid DepartmentId { get; set; }

    public string DepartmentName { get; set; } = string.Empty;

    public string ConsultationType { get; set; } = string.Empty;

    public string? ReasonForVisit { get; set; }

    public string? Notes { get; set; }

    public bool IsFirstVisit { get; set; }

    public Guid AppointmentStatusId { get; set; }

    public string AppointmentStatusName { get; set; } = string.Empty;
}