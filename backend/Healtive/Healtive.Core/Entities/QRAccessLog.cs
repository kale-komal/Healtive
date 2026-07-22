namespace Healtive.Core.Entities;

public class QRAccessLog
{
    public Guid Id { get; set; }

    public Guid PatientId { get; set; }

    public Guid UserId { get; set; }

    public string Action { get; set; } = string.Empty;

    public DateTime AccessTime { get; set; }

    public string? Remarks { get; set; }
}