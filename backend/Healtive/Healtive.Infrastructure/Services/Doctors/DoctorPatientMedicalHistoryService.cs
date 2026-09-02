using Healtive.Application.DTOs.Doctor.PatientMedicalHistory;
using Healtive.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Healtive.Infrastructure.Services.Doctors;

public class DoctorPatientMedicalHistoryService
    : IDoctorPatientMedicalHistoryService
{
    private readonly IDoctorPatientMedicalHistoryRepository _repository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DoctorPatientMedicalHistoryService(
        IDoctorPatientMedicalHistoryRepository repository,
        IDoctorRepository doctorRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _doctorRepository = doctorRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    // =========================================================
    // GET PATIENT MEDICAL HISTORY
    // =========================================================

    public async Task<DoctorPatientMedicalHistoryPageResponse?>
        GetMedicalHistoryAsync(
            Guid patientId,
            DoctorPatientMedicalHistoryFilterRequest request)
    {
        var hospitalId = GetHospitalId();

        var userId = GetUserId();

        // =====================================================
        // GET LOGGED-IN DOCTOR
        // =====================================================

        var doctor =
            await _doctorRepository.GetByUserIdAsync(
                hospitalId,
                userId);

        if (doctor == null)
        {
            throw new UnauthorizedAccessException(
                "Doctor profile not found.");
        }
        // =====================================================
        // VERIFY DOCTOR-PATIENT ASSOCIATION
        // =====================================================

        var isAssociated =
            await _repository.IsPatientAssociatedWithDoctorAsync(
                hospitalId,
                doctor.Id,
                patientId);

        if (!isAssociated)
        {
            return null;
        }
        // =====================================================
        // VALIDATE PATIENT
        // =====================================================

        var summary =
            await _repository.GetSummaryAsync(
                hospitalId,
                patientId);

        if (summary == null)
        {
            return null;
        }

        // =====================================================
        // GET HISTORY
        // =====================================================

        var history =
            await _repository.GetHistoryAsync(
                hospitalId,
                patientId,
                request);

        // =====================================================
        // RESPONSE
        // =====================================================

        return new DoctorPatientMedicalHistoryPageResponse
        {
            Summary = summary,

            History = history
        };
    }

    // =========================================================
    // GET HOSPITAL ID FROM JWT
    // =========================================================

    private Guid GetHospitalId()
    {
        var value =
            _httpContextAccessor
                .HttpContext?
                .User
                .FindFirst("HospitalId")
                ?.Value;

        if (!Guid.TryParse(
                value,
                out var hospitalId))
        {
            throw new UnauthorizedAccessException(
                "HospitalId claim is missing.");
        }

        return hospitalId;
    }

    // =========================================================
    // GET USER ID FROM JWT
    // =========================================================

    private Guid GetUserId()
    {
        var value =
            _httpContextAccessor
                .HttpContext?
                .User
                .FindFirst(
                    ClaimTypes.NameIdentifier)
                ?.Value;

        if (!Guid.TryParse(
                value,
                out var userId))
        {
            throw new UnauthorizedAccessException(
                "UserId claim is missing.");
        }

        return userId;
    }
}