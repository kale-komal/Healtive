namespace Healtive.Core.Entities;

public class AppointmentAttachment
{
    public Guid Id { get; set; }

    public Guid AppointmentId { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string FileUrl { get; set; } = string.Empty;

    public string? FileType { get; set; }

    public Guid UploadedByUserId { get; set; }

    public DateTime UploadedAt { get; set; }
}