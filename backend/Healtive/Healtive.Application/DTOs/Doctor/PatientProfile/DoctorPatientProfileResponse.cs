namespace Healtive.Application.DTOs.Doctor.PatientProfile;

public class DoctorPatientProfileResponse
{
    public Guid PatientId { get; set; }

    public string PatientCode { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public DateOnly? DateOfBirth { get; set; }

    public int? Age { get; set; }

    public string Gender { get; set; } = string.Empty;

    public string? BloodGroup { get; set; }

    public string MobileNumber { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? ProfileImageUrl { get; set; }

    public DoctorPatientEmergencyContactResponse?
        EmergencyContact
    { get; set; }

    public DoctorPatientAddressResponse?
        Address
    { get; set; }

    public DoctorPatientInsuranceResponse?
        Insurance
    { get; set; }
}