namespace Healtive.Core.Entities;

public class Hospital
{
    public Guid Id { get; set; }

    public string HospitalCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? RegistrationNumber { get; set; }

    public string? Email { get; set; }

    public string? MobileNumber { get; set; }

    public string? PhoneNumber { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public string? PostalCode { get; set; }

    public string? LogoUrl { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }
}