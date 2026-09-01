namespace Healtive.Application.DTOs.Doctor.PatientProfile;

public class DoctorPatientEmergencyContactResponse
{
    public string Name { get; set; } = string.Empty;

    public string Relationship { get; set; } = string.Empty;

    public string MobileNumber { get; set; } = string.Empty;

    public string? AlternateNumber { get; set; }

    public string? Address { get; set; }
}