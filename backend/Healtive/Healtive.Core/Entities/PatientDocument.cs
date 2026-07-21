namespace Healtive.Core.Entities;

public class PatientDocument
{
    public Guid Id { get; set; }

    public Guid PatientId { get; set; }

    public string DocumentType { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;

    public string FileUrl { get; set; } = string.Empty;

    public DateTime UploadedAt { get; set; }

    public Guid? UploadedByUserId { get; set; }
}