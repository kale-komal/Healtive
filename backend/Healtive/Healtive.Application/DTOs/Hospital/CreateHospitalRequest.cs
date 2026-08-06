public class CreateHospitalRequest
{
    public string Name { get; set; } = string.Empty;

    //public string Code { get; set; } = string.Empty;

    public string? LicenseNumber { get; set; }

    public string? GSTNumber { get; set; }

    public string HospitalType { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Website { get; set; }

    public string Address { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;

    public string? TimeZone { get; set; }

    public string? Currency { get; set; }
}