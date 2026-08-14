using Healtive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Healtive.API.Controllers;

[Authorize(Roles = "HospitalAdmin")]
[ApiController]
[Route("api/hospital")]
public class HospitalAdminController : ControllerBase
{
    private readonly ICurrentUserService _currentUser;

    public HospitalAdminController(
        ICurrentUserService currentUser)
    {
        _currentUser = currentUser;
    }

    [HttpGet("context")]
    public IActionResult GetContext()
    {
        return Ok(new
        {
            UserId = _currentUser.UserId,
            HospitalId = _currentUser.HospitalId,
            BranchId = _currentUser.BranchId,
            Role = _currentUser.Role
        });
    }
}