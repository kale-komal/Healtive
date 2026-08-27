namespace Healtive.Application.DTOs.Appointment;

public class AppointmentHistoryResponse
{
    public Guid Id { get; set; }

    public Guid AppointmentId { get; set; }

    public Guid AppointmentStatusId { get; set; }

    public string? AppointmentStatusName { get; set; }

    public Guid ChangedByUserId { get; set; }

    public string? Remarks { get; set; }

    public DateTime ChangedAt { get; set; }
}