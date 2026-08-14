using System.Security.Claims;
using Healtive.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Healtive.Infrastructure.Services.Auth;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?
                .User
                .FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(value, out var id)
                ? id
                : Guid.Empty;
        }
    }

    public Guid HospitalId
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?
                .User
                .FindFirst("HospitalId")?
                .Value;

            return Guid.TryParse(value, out var id)
                ? id
                : Guid.Empty;
        }
    }

    public Guid? BranchId
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?
                .User
                .FindFirst("BranchId")?
                .Value;

            if (Guid.TryParse(value, out var id))
                return id;

            return null;
        }
    }

    public string Role
    {
        get
        {
            return _httpContextAccessor.HttpContext?
                .User
                .FindFirstValue(ClaimTypes.Role)
                ?? string.Empty;
        }
    }
}