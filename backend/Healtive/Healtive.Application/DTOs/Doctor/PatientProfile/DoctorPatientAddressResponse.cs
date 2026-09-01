namespace Healtive.Application.DTOs.Doctor.PatientProfile;

public class DoctorPatientAddressResponse
{
    public string AddressType { get; set; } = string.Empty;

    public string AddressLine1 { get; set; } = string.Empty;

    public string? AddressLine2 { get; set; }

    public string City { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public string? PostalCode { get; set; }

    public bool IsDefault { get; set; }
}