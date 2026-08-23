namespace Healtive.Application.DTOs.DoctorAvailability;

public class CreateDoctorAvailabilityRequest
{
    public byte DayOfWeek { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public int MaxAppointments { get; set; }
}