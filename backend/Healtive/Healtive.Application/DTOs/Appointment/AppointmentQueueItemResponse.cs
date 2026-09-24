namespace Healtive.Application.DTOs.Appointment;

public class AppointmentQueueItemResponse
{
    public Guid AppointmentId { get; set; }

    public string AppointmentNumber { get; set; } = string.Empty;

    public Guid PatientId { get; set; }

    public string PatientName { get; set; } = string.Empty;

    public string PatientCode { get; set; } = string.Empty;

    public Guid DoctorId { get; set; }

    public string DoctorName { get; set; } = string.Empty;

    public DateOnly AppointmentDate { get; set; }

    public TimeSpan AppointmentTime { get; set; }

    public int? TokenNumber { get; set; }

    public string AppointmentStatus { get; set; } = string.Empty;

    public string AppointmentStatusCode { get; set; } = string.Empty;

    public string ConsultationType { get; set; } = string.Empty;

    public bool IsFirstVisit { get; set; }
}