namespace Healtive.Application.DTOs.Doctor.FollowUp;

public class CreateFollowUpRequest
{
    public Guid AppointmentId { get; set; }

    public DateOnly FollowUpDate { get; set; }

    public string? FollowUpNotes { get; set; }
}