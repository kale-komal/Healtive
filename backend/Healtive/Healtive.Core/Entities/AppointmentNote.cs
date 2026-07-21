namespace Healtive.Core.Entities;

public class AppointmentNote
{
    public Guid Id { get; set; }

    public Guid AppointmentId { get; set; }

    public string Note { get; set; } = string.Empty;

    public Guid CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }
}