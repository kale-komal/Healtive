namespace Healtive.Application.DTOs.Appointment;

public enum AppointmentCheckInOutcome
{
    NotFound,

    AlreadyCheckedIn,

    TerminalStatus,

    StatusNotConfigured,

    Success
}

public class AppointmentCheckInResult
{
    public AppointmentCheckInOutcome Outcome { get; set; }

    public string? AppointmentStatusCode { get; set; }

    public AppointmentResponse? Appointment { get; set; }
}