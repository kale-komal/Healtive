namespace Healtive.Application.DTOs.Doctor.Lab;

public class CreateLabOrderRequest
{
    public Guid AppointmentId { get; set; }

    public List<Guid> TestIds { get; set; } = new();

    public string? Remarks { get; set; }
}