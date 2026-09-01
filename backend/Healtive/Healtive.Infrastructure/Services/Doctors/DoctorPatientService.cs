using Healtive.Application.DTOs.Doctor.PatientProfile;
using Healtive.Application.Interfaces;

namespace Healtive.Infrastructure.Services.Doctors;

public class DoctorPatientService : IDoctorPatientService
{
    private readonly IDoctorPatientRepository _repository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly ICurrentUserService _currentUserService;

    public DoctorPatientService(
        IDoctorPatientRepository repository,
        IDoctorRepository doctorRepository,
        ICurrentUserService currentUserService)
    {
        _repository = repository;
        _doctorRepository = doctorRepository;
        _currentUserService = currentUserService;
    }

    // =========================================================
    // GET PATIENT PROFILE
    // =========================================================

    public async Task<DoctorPatientProfileResponse?>
        GetPatientProfileAsync(
            Guid patientId)
    {
        // =====================================================
        // GET CURRENT HOSPITAL
        // =====================================================

        var hospitalId =
            _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            throw new UnauthorizedAccessException(
                "Hospital context not found.");
        }

        // =====================================================
        // GET CURRENT USER
        // =====================================================

        var userId =
            _currentUserService.UserId;

        if (userId == Guid.Empty)
        {
            throw new UnauthorizedAccessException(
                "User context not found.");
        }

        // =====================================================
        // GET DOCTOR FROM LOGGED-IN USER
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
        // VALIDATE PATIENT ID
        // =====================================================

        if (patientId == Guid.Empty)
        {
            return null;
        }

        // =====================================================
        // GET PATIENT PROFILE
        // =====================================================

        return await _repository.GetPatientProfileAsync(
            hospitalId,
            doctor.Id,
            patientId);
    }
}