namespace Healtive.Core.Entities;

public class PatientQRCode
{
    public Guid Id { get; set; }

    public Guid PatientId { get; set; }

    public Guid QRToken { get; set; }

    public string? QRImageUrl { get; set; }

    public bool IsActive { get; set; }

    public DateTime GeneratedAt { get; set; }
}