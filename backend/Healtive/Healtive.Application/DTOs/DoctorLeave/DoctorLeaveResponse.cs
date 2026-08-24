namespace Healtive.Application.DTOs.DoctorLeave;

public class DoctorLeaveResponse
{
    public Guid Id { get; set; }

    public Guid DoctorId { get; set; }

    public DateOnly FromDate { get; set; }

    public DateOnly ToDate { get; set; }

    public string? Reason { get; set; }

    public bool IsApproved { get; set; }

    public DateTime CreatedAt { get; set; }
}