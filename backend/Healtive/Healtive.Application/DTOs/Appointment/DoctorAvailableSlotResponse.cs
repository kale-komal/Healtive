namespace Healtive.Application.DTOs.Appointment;

public class DoctorAvailableSlotResponse
{
    public DateOnly AppointmentDate { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public bool IsAvailable { get; set; }
}