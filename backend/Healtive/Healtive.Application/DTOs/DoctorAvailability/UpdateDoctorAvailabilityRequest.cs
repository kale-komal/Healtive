namespace Healtive.Application.DTOs.DoctorAvailability;

public class UpdateDoctorAvailabilityRequest
{
    public byte DayOfWeek { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public int MaxAppointments { get; set; }

    public bool IsAvailable { get; set; }
}