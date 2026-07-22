namespace Healtive.Core.Entities;

public class QRDevice
{
    public Guid Id { get; set; }

    public Guid HospitalId { get; set; }

    public Guid BranchId { get; set; }

    public string DeviceName { get; set; } = string.Empty;

    public string? DeviceIdentifier { get; set; }

    public bool IsActive { get; set; }

    public DateTime RegisteredAt { get; set; }
}