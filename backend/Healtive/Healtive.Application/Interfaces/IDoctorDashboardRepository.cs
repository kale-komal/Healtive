using Healtive.Application.DTOs.Doctor.DoctorDashboard;

namespace Healtive.Application.Interfaces;

public interface IDoctorDashboardRepository
{
    Task<DoctorDashboardResponse?> GetDashboardAsync(
        Guid hospitalId,
        Guid doctorId);

    Task<IEnumerable<DoctorAppointmentResponse>>
        GetTodayAppointmentsAsync(
            Guid hospitalId,
            Guid doctorId);
}