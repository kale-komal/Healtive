using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Dashboard;
using Healtive.Application.Interfaces;

namespace Healtive.Infrastructure.Services.Dashboard;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _dashboardRepository;

    public DashboardService(
        IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }

    public async Task<ApiResponse<DashboardResponse>> GetDashboardAsync()
    {
        var dashboard = await _dashboardRepository.GetDashboardAsync();

        return ApiResponse<DashboardResponse>.SuccessResponse(
            dashboard,
            "Dashboard data fetched successfully.");
    }
}