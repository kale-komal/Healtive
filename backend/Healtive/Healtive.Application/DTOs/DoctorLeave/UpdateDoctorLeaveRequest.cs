namespace Healtive.Application.DTOs.DoctorLeave;

public class UpdateDoctorLeaveRequest
{
    public DateOnly FromDate { get; set; }

    public DateOnly ToDate { get; set; }

    public string? Reason { get; set; }
}