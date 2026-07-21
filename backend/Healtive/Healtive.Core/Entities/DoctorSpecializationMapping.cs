namespace Healtive.Core.Entities;

public class DoctorSpecializationMapping
{
    public Guid DoctorId { get; set; }

    public Guid SpecializationId { get; set; }

    public DateTime CreatedAt { get; set; }
}