namespace Healtive.Application.DTOs.DoctorSpecialization;

public class DoctorSpecializationResponse
{
    public Guid SpecializationId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}