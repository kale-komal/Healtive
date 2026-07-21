namespace Healtive.Core.Entities;

public class AppointmentHistory
{
    public Guid Id { get; set; }

    public Guid AppointmentId { get; set; }

    public Guid AppointmentStatusId { get; set; }

    public Guid ChangedByUserId { get; set; }

    public string? Remarks { get; set; }

    public DateTime ChangedAt { get; set; }
}
