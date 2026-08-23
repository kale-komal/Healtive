namespace Healtive.Application.DTOs.DoctorAvailability;

public class DoctorAvailabilityResponse
{
    public Guid Id { get; set; }

    public Guid DoctorId { get; set; }

    public byte DayOfWeek { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public int MaxAppointments { get; set; }

    public bool IsAvailable { get; set; }

    public DateTime CreatedAt { get; set; }
}