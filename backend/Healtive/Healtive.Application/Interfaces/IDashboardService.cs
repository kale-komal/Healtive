using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Dashboard;

namespace Healtive.Application.Interfaces;

public interface IDashboardService
{
    Task<ApiResponse<DashboardResponse>> GetDashboardAsync();
}