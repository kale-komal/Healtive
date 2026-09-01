using Healtive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Healtive.API.Controllers.Hospital;

[ApiController]
[Route("api/doctor/dashboard")]
[Authorize(Roles = "Doctor")]
public class DoctorDashboardController : ControllerBase
{
    private readonly IDoctorDashboardService _dashboardService;

    public DoctorDashboardController(
        IDoctorDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    // =========================================================
    // GET DOCTOR DASHBOARD
    // =========================================================

    [HttpGet]
    public async Task<IActionResult> GetDashboard()
    {
        var response =
            await _dashboardService.GetDashboardAsync();

        if (response == null)
        {
            return NotFound(new
            {
                message = "Doctor dashboard not found."
            });
        }

        return Ok(response);
    }

    // =========================================================
    // GET TODAY'S APPOINTMENTS
    // =========================================================

    [HttpGet("today-appointments")]
    public async Task<IActionResult> GetTodayAppointments()
    {
        var response =
            await _dashboardService.GetTodayAppointmentsAsync();

        return Ok(response);
    }
}