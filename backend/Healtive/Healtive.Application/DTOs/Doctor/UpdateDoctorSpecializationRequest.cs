namespace Healtive.Application.DTOs.Doctor;

public class UpdateDoctorSpecializationRequest
{
    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; }
}