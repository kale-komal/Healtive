using Healtive.Application.DTOs.Doctor.DoctorDashboard;
using Healtive.Application.Interfaces;
using Healtive.Infrastructure.Repositories.Doctors;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Healtive.Infrastructure.Services.Doctors;

public class DoctorDashboardService : IDoctorDashboardService
{
    private readonly IDoctorDashboardRepository _repository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DoctorDashboardService(
     IDoctorDashboardRepository repository,
     IDoctorRepository doctorRepository,
     IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _doctorRepository = doctorRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<DoctorDashboardResponse?> GetDashboardAsync()
    {
        var hospitalId = GetHospitalId();
        var userId = GetUserId();

        var doctorId = await GetDoctorIdAsync(
            hospitalId,
            userId);

        if (doctorId == null)
            return null;

        return await _repository.GetDashboardAsync(
            hospitalId,
            doctorId.Value);
    }

    public async Task<IEnumerable<DoctorAppointmentResponse>>
        GetTodayAppointmentsAsync()
    {
        var hospitalId = GetHospitalId();
        var userId = GetUserId();

        var doctorId = await GetDoctorIdAsync(
            hospitalId,
            userId);

        if (doctorId == null)
            return Enumerable.Empty<DoctorAppointmentResponse>();

        return await _repository.GetTodayAppointmentsAsync(
            hospitalId,
            doctorId.Value);
    }

    // =========================================================
    // GET DOCTOR ID FROM LOGGED-IN USER
    // =========================================================

    private async Task<Guid?> GetDoctorIdAsync(
    Guid hospitalId,
    Guid userId)
    {
        var doctor = await _doctorRepository.GetByUserIdAsync(
            hospitalId,
            userId);

        return doctor?.Id;
    }

    // =========================================================
    // JWT CLAIMS
    // =========================================================

    private Guid GetHospitalId()
    {
        var value = _httpContextAccessor
            .HttpContext?
            .User
            .FindFirst("HospitalId")
            ?.Value;

        if (!Guid.TryParse(value, out var hospitalId))
            throw new UnauthorizedAccessException(
                "HospitalId claim is missing.");

        return hospitalId;
    }

    private Guid GetUserId()
    {
        var value = _httpContextAccessor
            .HttpContext?
            .User
            .FindFirst(ClaimTypes.NameIdentifier)
            ?.Value;

        if (!Guid.TryParse(value, out var userId))
            throw new UnauthorizedAccessException(
                "UserId claim is missing.");

        return userId;
    }
}