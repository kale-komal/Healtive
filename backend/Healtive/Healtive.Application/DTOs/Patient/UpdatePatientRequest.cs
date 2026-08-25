namespace Healtive.Application.DTOs.Patient;

public class UpdatePatientRequest
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateOnly? DateOfBirth { get; set; }

    public string Gender { get; set; } = string.Empty;

    public string? BloodGroup { get; set; }

    public string MobileNumber { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? ProfileImageUrl { get; set; }
}
