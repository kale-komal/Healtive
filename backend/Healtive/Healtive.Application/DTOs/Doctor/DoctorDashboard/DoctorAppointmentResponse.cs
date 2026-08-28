namespace Healtive.Application.DTOs.Doctor.DoctorDashboard;

public class DoctorAppointmentResponse
{
    public Guid Id { get; set; }

    public string AppointmentNumber { get; set; } = string.Empty;

    public int? TokenNumber { get; set; }

    public Guid PatientId { get; set; }

    public string PatientCode { get; set; } = string.Empty;

    public string PatientName { get; set; } = string.Empty;

    public string? PatientMobileNumber { get; set; }

    public Guid DepartmentId { get; set; }

    public string DepartmentName { get; set; } = string.Empty;

    public DateOnly AppointmentDate { get; set; }

    public TimeSpan AppointmentTime { get; set; }

    public string ConsultationType { get; set; } = string.Empty;

    public string? ReasonForVisit { get; set; }

    public bool IsFirstVisit { get; set; }

    public Guid AppointmentStatusId { get; set; }

    public string AppointmentStatusName { get; set; } = string.Empty;
}