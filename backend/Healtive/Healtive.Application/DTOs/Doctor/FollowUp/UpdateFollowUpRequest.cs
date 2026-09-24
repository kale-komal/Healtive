namespace Healtive.Application.DTOs.Doctor.FollowUp;

public class UpdateFollowUpRequest
{
    public DateOnly FollowUpDate { get; set; }

    public string? FollowUpNotes { get; set; }

    public string Status { get; set; } = string.Empty;
}