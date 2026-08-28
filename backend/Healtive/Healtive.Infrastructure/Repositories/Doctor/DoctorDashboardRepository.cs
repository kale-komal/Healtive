using Dapper;
using Healtive.Application.DTOs.Doctor.DoctorDashboard;
using Healtive.Application.Interfaces;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.Doctors;

public class DoctorDashboardRepository : IDoctorDashboardRepository
{
    private readonly IDbConnectionFactory _db;

    public DoctorDashboardRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<DoctorDashboardResponse?> GetDashboardAsync(
        Guid hospitalId,
        Guid doctorId)
    {
        using var connection = _db.CreateConnection();

        // =========================================================
        // GET DOCTOR
        // =========================================================

        const string doctorSql = @"
SELECT
    Id,
    FullName
FROM Doctors
WHERE Id = @DoctorId
AND HospitalId = @HospitalId
AND IsActive = 1
AND IsDeleted = 0;";

        var doctor = await connection.QueryFirstOrDefaultAsync<DoctorDbModel>(
            doctorSql,
            new
            {
                DoctorId = doctorId,
                HospitalId = hospitalId
            });

        if (doctor == null)
            return null;

        // =========================================================
        // TODAY'S DATE
        // =========================================================

        var today = DateOnly.FromDateTime(DateTime.Now);

        var todayDate = today.ToDateTime(TimeOnly.MinValue);

        // =========================================================
        // DASHBOARD COUNTS
        // =========================================================

        const string countSql = @"
SELECT
    COUNT(*) AS TotalAppointments,

    SUM(
        CASE
            WHEN s.Code IN ('WAITING', 'CHECKED_IN', 'CALLED')
            THEN 1
            ELSE 0
        END
    ) AS WaitingAppointments,

    SUM(
        CASE
            WHEN s.Code = 'COMPLETED'
            THEN 1
            ELSE 0
        END
    ) AS CompletedAppointments,

    SUM(
        CASE
            WHEN a.AppointmentTime > CURTIME()
            AND s.Code NOT IN
            (
                'CANCELLED',
                'COMPLETED',
                'NO_SHOW'
            )
            THEN 1
            ELSE 0
        END
    ) AS UpcomingAppointments

FROM Appointments a

INNER JOIN AppointmentStatuses s
    ON s.Id = a.AppointmentStatusId

WHERE a.HospitalId = @HospitalId
AND a.DoctorId = @DoctorId
AND a.AppointmentDate = @AppointmentDate;";

        var counts = await connection.QueryFirstAsync<DoctorDashboardCountDbModel>(
            countSql,
            new
            {
                HospitalId = hospitalId,
                DoctorId = doctorId,
                AppointmentDate = todayDate
            });

        // =========================================================
        // TODAY'S APPOINTMENTS
        // =========================================================

        const string appointmentsSql = @"
SELECT
    a.Id,
    a.AppointmentNumber,
    a.TokenNumber,

    a.PatientId,
    p.PatientCode,

    CONCAT(
        p.FirstName,
        ' ',
        p.LastName
    ) AS PatientName,

    p.MobileNumber AS PatientMobileNumber,

    a.DepartmentId,
    dep.Name AS DepartmentName,

    a.AppointmentDate,
    a.AppointmentTime,

    a.ConsultationType,
    a.ReasonForVisit,
    a.IsFirstVisit,

    a.AppointmentStatusId,
    s.Name AS AppointmentStatusName

FROM Appointments a

INNER JOIN Patients p
    ON p.Id = a.PatientId

INNER JOIN Departments dep
    ON dep.Id = a.DepartmentId

INNER JOIN AppointmentStatuses s
    ON s.Id = a.AppointmentStatusId

WHERE a.HospitalId = @HospitalId
AND a.DoctorId = @DoctorId
AND a.AppointmentDate = @AppointmentDate

ORDER BY
    a.AppointmentTime ASC,
    a.TokenNumber ASC;";

        var appointments =
            await connection.QueryAsync<DoctorAppointmentResponse>(
                appointmentsSql,
                new
                {
                    HospitalId = hospitalId,
                    DoctorId = doctorId,
                    AppointmentDate = todayDate
                });

        // =========================================================
        // RESPONSE
        // =========================================================

        return new DoctorDashboardResponse
        {
            DoctorId = doctor.Id,

            DoctorName = doctor.FullName,

            TotalAppointments = counts.TotalAppointments,

            WaitingAppointments = counts.WaitingAppointments,

            CompletedAppointments = counts.CompletedAppointments,

            UpcomingAppointments = counts.UpcomingAppointments,

            TodayAppointments = appointments
        };
    }

    public async Task<IEnumerable<DoctorAppointmentResponse>>
        GetTodayAppointmentsAsync(
            Guid hospitalId,
            Guid doctorId)
    {
        using var connection = _db.CreateConnection();

        var today = DateOnly.FromDateTime(DateTime.Now);

        const string sql = @"
SELECT
    a.Id,
    a.AppointmentNumber,
    a.TokenNumber,

    a.PatientId,
    p.PatientCode,

    CONCAT(
        p.FirstName,
        ' ',
        p.LastName
    ) AS PatientName,

    p.MobileNumber AS PatientMobileNumber,

    a.DepartmentId,
    dep.Name AS DepartmentName,

    a.AppointmentDate,
    a.AppointmentTime,

    a.ConsultationType,
    a.ReasonForVisit,
    a.IsFirstVisit,

    a.AppointmentStatusId,
    s.Name AS AppointmentStatusName

FROM Appointments a

INNER JOIN Patients p
    ON p.Id = a.PatientId

INNER JOIN Departments dep
    ON dep.Id = a.DepartmentId

INNER JOIN AppointmentStatuses s
    ON s.Id = a.AppointmentStatusId

WHERE a.HospitalId = @HospitalId
AND a.DoctorId = @DoctorId
AND a.AppointmentDate = @AppointmentDate

ORDER BY
    a.AppointmentTime ASC,
    a.TokenNumber ASC;";

        return await connection.QueryAsync<DoctorAppointmentResponse>(
            sql,
            new
            {
                HospitalId = hospitalId,
                DoctorId = doctorId,
                AppointmentDate =
                    today.ToDateTime(TimeOnly.MinValue)
            });
    }

    // =============================================================
    // PRIVATE DB MODELS
    // =============================================================

    private class DoctorDbModel
    {
        public Guid Id { get; set; }

        public string FullName { get; set; } = string.Empty;
    }

    private class DoctorDashboardCountDbModel
    {
        public int TotalAppointments { get; set; }

        public int WaitingAppointments { get; set; }

        public int CompletedAppointments { get; set; }

        public int UpcomingAppointments { get; set; }
    }
}