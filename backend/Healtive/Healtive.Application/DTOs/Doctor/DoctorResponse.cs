namespace Healtive.Application.DTOs.Doctor;

public class DoctorResponse
{
    public Guid DoctorId { get; set; }

    public Guid HospitalId { get; set; }

    public Guid? UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string DoctorCode { get; set; } = string.Empty;

    public string RegistrationNumber { get; set; } = string.Empty;

    public string Qualification { get; set; } = string.Empty;

    public int ExperienceYears { get; set; }

    public decimal ConsultationFee { get; set; }

    public string Gender { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }

    public DateTime? JoiningDate { get; set; }

    public string? Bio { get; set; }

    public string? ProfileImageUrl { get; set; }

    public bool IsAvailable { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}