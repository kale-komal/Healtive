using Dapper;
using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Doctor.PatientMedicalHistory;
using Healtive.Application.Interfaces;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.Doctors;

public class DoctorPatientMedicalHistoryRepository
    : IDoctorPatientMedicalHistoryRepository
{
    private readonly IDbConnectionFactory _db;

    public DoctorPatientMedicalHistoryRepository(
        IDbConnectionFactory db)
    {
        _db = db;
    }

    // =========================================================
    // GET PATIENT MEDICAL HISTORY SUMMARY
    // =========================================================

    public async Task<DoctorPatientMedicalHistorySummaryResponse?>
    GetSummaryAsync(
        Guid hospitalId,
        Guid patientId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    p.Id AS PatientId,

    p.PatientCode,

    CONCAT(
        p.FirstName,
        ' ',
        p.LastName
    ) AS PatientName,

    COUNT(a.Id) AS TotalVisits,

    MIN(a.AppointmentDate) AS FirstVisitDate,

    MAX(a.AppointmentDate) AS LastVisitDate

FROM Patients p

INNER JOIN Appointments a
    ON a.PatientId = p.Id
    AND a.HospitalId = @HospitalId

WHERE p.Id = @PatientId
AND p.IsActive = 1

GROUP BY
    p.Id,
    p.PatientCode,
    p.FirstName,
    p.LastName;";

        return await connection.QueryFirstOrDefaultAsync<
            DoctorPatientMedicalHistorySummaryResponse>(
            sql,
            new
            {
                HospitalId = hospitalId,
                PatientId = patientId
            });
    }

    // =========================================================
    // GET PATIENT MEDICAL HISTORY
    // =========================================================

    public async Task<PagedResponse<DoctorPatientMedicalHistoryResponse>>
        GetHistoryAsync(
            Guid hospitalId,
            Guid patientId,
            DoctorPatientMedicalHistoryFilterRequest request)
    {
        using var connection = _db.CreateConnection();

        // =====================================================
        // PAGINATION
        // =====================================================

        var page = request.Page < 1
            ? 1
            : request.Page;

        var pageSize = request.PageSize < 1
            ? 10
            : request.PageSize;

        if (pageSize > 100)
            pageSize = 100;

        var offset =
            (page - 1) * pageSize;

        // =====================================================
        // TOTAL RECORDS
        // =====================================================

        const string countSql = @"
SELECT COUNT(*)

FROM Appointments a

WHERE a.HospitalId = @HospitalId

AND a.PatientId = @PatientId;";

        var totalRecords =
            await connection.ExecuteScalarAsync<int>(
                countSql,
                new
                {
                    HospitalId = hospitalId,
                    PatientId = patientId
                });

        // =====================================================
        // HISTORY
        // =====================================================

        const string sql = @"
SELECT
    a.Id AS AppointmentId,

    a.AppointmentNumber,

    a.AppointmentDate,

    a.AppointmentTime,

    a.DoctorId,

    d.FullName AS DoctorName,

    a.DepartmentId,

    dep.Name AS DepartmentName,

    a.ConsultationType,

    a.ReasonForVisit,

    a.Notes,

    a.IsFirstVisit,

    a.AppointmentStatusId,

    s.Name AS AppointmentStatusName

FROM Appointments a

INNER JOIN Doctors d
    ON d.Id = a.DoctorId

INNER JOIN Departments dep
    ON dep.Id = a.DepartmentId

INNER JOIN AppointmentStatuses s
    ON s.Id = a.AppointmentStatusId

WHERE a.HospitalId = @HospitalId

AND a.PatientId = @PatientId

ORDER BY
    a.AppointmentDate DESC,
    a.AppointmentTime DESC

LIMIT @PageSize
OFFSET @Offset;";

        var history =
            await connection.QueryAsync<
                DoctorPatientMedicalHistoryResponse>(
                sql,
                new
                {
                    HospitalId = hospitalId,
                    PatientId = patientId,
                    PageSize = pageSize,
                    Offset = offset
                });

        // =====================================================
        // RESPONSE
        // =====================================================

        var totalPages =
            totalRecords == 0
                ? 0
                : (int)Math.Ceiling(
                    totalRecords /
                    (double)pageSize);

        return new PagedResponse<
            DoctorPatientMedicalHistoryResponse>
        {
            Items = history,

            Page = page,

            PageSize = pageSize,

            TotalCount = totalRecords,

            TotalPages = totalPages
        };
    }

    // =========================================================
    // VERIFY DOCTOR-PATIENT ASSOCIATION
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