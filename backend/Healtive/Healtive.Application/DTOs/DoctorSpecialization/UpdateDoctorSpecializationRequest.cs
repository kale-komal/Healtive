namespace Healtive.Application.DTOs.DoctorSpecialization;

public class UpdateDoctorSpecializationRequest
{
    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }
}