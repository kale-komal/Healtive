using Healtive.Application.DTOs.Doctor.DoctorDashboard;

namespace Healtive.Application.Interfaces;

public interface IDoctorDashboardService
{
    Task<DoctorDashboardResponse?> GetDashboardAsync();

    Task<IEnumerable<DoctorAppointmentResponse>>
        GetTodayAppointmentsAsync();
}