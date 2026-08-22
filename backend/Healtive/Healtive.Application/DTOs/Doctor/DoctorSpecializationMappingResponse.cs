namespace Healtive.Application.DTOs.Doctor;

public class DoctorSpecializationMappingResponse
{
    public Guid DoctorId { get; set; }

    public Guid SpecializationId { get; set; }

    public string SpecializationName { get; set; } = string.Empty;

    public string SpecializationCode { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}