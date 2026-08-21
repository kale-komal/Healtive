namespace Healtive.Application.DTOs.Doctor;

public class DoctorListResponse
{
    public Guid DoctorId { get; set; }

    public Guid? UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string DoctorCode { get; set; } = string.Empty;

    public string RegistrationNumber { get; set; } = string.Empty;

    public string Qualification { get; set; } = string.Empty;

    public int ExperienceYears { get; set; }

    public decimal ConsultationFee { get; set; }

    public string Gender { get; set; } = string.Empty;

    public bool IsAvailable { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}