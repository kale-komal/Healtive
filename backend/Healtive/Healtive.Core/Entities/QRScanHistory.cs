namespace Healtive.Core.Entities;

public class QRScanHistory
{
    public Guid Id { get; set; }

    public Guid PatientQRCodeId { get; set; }

    public Guid HospitalId { get; set; }

    public Guid BranchId { get; set; }

    public Guid ScannedByUserId { get; set; }

    public DateTime ScanTime { get; set; }

    public string? DeviceInfo { get; set; }

    public string? IPAddress { get; set; }

    public string? Remarks { get; set; }
}