using Healtive.Application.DTOs.Dashboard;

namespace Healtive.Application.Interfaces;

public interface IDashboardRepository
{
    Task<DashboardResponse> GetDashboardAsync();
}