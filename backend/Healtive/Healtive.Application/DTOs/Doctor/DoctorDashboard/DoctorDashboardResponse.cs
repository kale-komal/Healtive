namespace Healtive.Application.DTOs.Doctor.DoctorDashboard;

public class DoctorDashboardResponse
{
    public Guid DoctorId { get; set; }

    public string DoctorName { get; set; } = string.Empty;

    public int TotalAppointments { get; set; }

    public int WaitingAppointments { get; set; }

    public int CompletedAppointments { get; set; }

    public int UpcomingAppointments { get; set; }

    public IEnumerable<DoctorAppointmentResponse> TodayAppointments { get; set; }
        = new List<DoctorAppointmentResponse>();
}