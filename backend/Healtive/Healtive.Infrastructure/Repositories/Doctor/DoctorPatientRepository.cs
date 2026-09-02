using Dapper;
using Healtive.Application.DTOs.Doctor.PatientProfile;
using Healtive.Application.Interfaces;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.Doctors;

public class DoctorPatientRepository : IDoctorPatientRepository
{
    private readonly IDbConnectionFactory _db;

    public DoctorPatientRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    // =========================================================
    // GET PATIENT PROFILE
    // =========================================================

    public async Task<DoctorPatientProfileResponse?>
        GetPatientProfileAsync(
            Guid hospitalId,
            Guid doctorId,
            Guid patientId)
    {
        using var connection = _db.CreateConnection();

        // =====================================================
        // VERIFY DOCTOR-PATIENT ASSOCIATION
        // =====================================================

        var isAssociated =
            await IsPatientAssociatedWithDoctorAsync(
                hospitalId,
                doctorId,
                patientId);

        if (!isAssociated)
            return null;

        // =====================================================
        // GET BASIC PATIENT INFORMATION
        // =====================================================

        const string sql = @"
SELECT
    p.Id AS PatientId,
    p.PatientCode,
    p.FirstName,
    p.LastName,

    CONCAT(
        p.FirstName,
        ' ',
        p.LastName
    ) AS FullName,

    p.DateOfBirth,

    CASE
        WHEN p.DateOfBirth IS NULL THEN NULL
        ELSE TIMESTAMPDIFF(
            YEAR,
            p.DateOfBirth,
            CURDATE()
        )
    END AS Age,

    p.Gender,
    p.BloodGroup,
    p.MobileNumber,
    p.Email,
    p.ProfileImageUrl

FROM Patients p

WHERE p.Id = @PatientId
AND p.IsActive = 1
AND p.IsDeleted = 0;";

        var patient =
            await connection.QueryFirstOrDefaultAsync<
                DoctorPatientProfileResponse>(
                sql,
                new
                {
                    PatientId = patientId
                });

        if (patient == null)
            return null;

        // =====================================================
        // GET ADDRESS
        // =====================================================

        patient.Address =
            await GetPatientAddressAsync(patientId);

        // =====================================================
        // GET EMERGENCY CONTACT
        // =====================================================

        patient.EmergencyContact =
            await GetEmergencyContactAsync(patientId);

        // =====================================================
        // GET INSURANCE
        // =====================================================

        patient.Insurance =
            await GetInsuranceAsync(patientId);

        return patient;
    }

    // =========================================================
    // GET PATIENT ADDRESS
    // =========================================================

    public async Task<DoctorPatientAddressResponse?>
        GetPatientAddressAsync(
            Guid patientId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    AddressType,
    AddressLine1,
    AddressLine2,
    City,
    State,
    Country,
    PostalCode,
    IsDefault

FROM PatientAddresses

WHERE PatientId = @PatientId

ORDER BY
    IsDefault DESC,
    CreatedAt DESC

LIMIT 1;";

        return await connection.QueryFirstOrDefaultAsync<
            DoctorPatientAddressResponse>(
            sql,
            new
            {
                PatientId = patientId
            });
    }

    // =========================================================
    // GET EMERGENCY CONTACT
    // =========================================================

    public async Task<DoctorPatientEmergencyContactResponse?>
        GetEmergencyContactAsync(
            Guid patientId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    Name,
    Relationship,
    MobileNumber,
    AlternateNumber,
    Address

FROM PatientEmergencyContacts

WHERE PatientId = @PatientId

ORDER BY CreatedAt DESC

LIMIT 1;";

        return await connection.QueryFirstOrDefaultAsync<
            DoctorPatientEmergencyContactResponse>(
            sql,
            new
            {
                PatientId = patientId
            });
    }

    // =========================================================
    // GET INSURANCE
    // =========================================================

    public async Task<DoctorPatientInsuranceResponse?>
        GetInsuranceAsync(
            Guid patientId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    InsuranceCompany,
    PolicyNumber,
    PolicyHolderName,
    ValidFrom,
    ValidTo,
    CoverageAmount,
    IsActive

FROM PatientInsurance

WHERE PatientId = @PatientId

AND IsActive = 1

ORDER BY CreatedAt DESC

LIMIT 1;";

        return await connection.QueryFirstOrDefaultAsync<
            DoctorPatientInsuranceResponse>(
            sql,
            new
            {
                PatientId = patientId
            });
    }

    // =========================================================
    // VERIFY PATIENT IS ASSOCIATED WITH DOCTOR
    // =========================================================

    public async Task<bool>
        IsPatientAssociatedWithDoctorAsync(
            Guid hospitalId,
            Guid doctorId,
            Guid patientId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)

FROM Appointments a

WHERE a.HospitalId = @HospitalId

AND a.DoctorId = @DoctorId

AND a.PatientId = @PatientId;";

        var count =
            await connection.ExecuteScalarAsync<int>(
                sql,
                new
                {
                    HospitalId = hospitalId,
                    DoctorId = doctorId,
                    PatientId = patientId
                });

        return count > 0;
    }
}