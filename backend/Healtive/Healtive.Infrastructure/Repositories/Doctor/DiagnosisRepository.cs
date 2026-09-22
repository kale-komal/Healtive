using Dapper;
using Healtive.Application.DTOs.Doctor.Diagnosis;
using Healtive.Application.Interfaces.Repositories;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.Doctors;

public class DiagnosisRepository : IDiagnosisRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DiagnosisRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    // ==========================================
    // GET DIAGNOSES BY APPOINTMENT
    // ==========================================

    public async Task<IEnumerable<DiagnosisResponse>> GetByAppointmentIdAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid appointmentId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"
            SELECT
                d.Id,
                d.AppointmentId,
                d.PatientId,

                CONCAT(
                    p.FirstName,
                    ' ',
                    COALESCE(p.LastName, '')
                ) AS PatientName,

                d.DoctorId,

                CONCAT(
                    doc.FirstName,
                    ' ',
                    COALESCE(doc.LastName, '')
                ) AS DoctorName,

                d.DiagnosisName,
                d.Description,
                d.Severity,
                d.Notes,
                d.DiagnosedAt,
                d.IsActive

            FROM Diagnoses d

            INNER JOIN Patients p
                ON p.Id = d.PatientId

            INNER JOIN Doctors doc
                ON doc.Id = d.DoctorId

            WHERE d.AppointmentId = @AppointmentId
              AND d.HospitalId = @HospitalId
              AND d.DoctorId = @DoctorId
              AND d.IsActive = TRUE

            ORDER BY d.DiagnosedAt DESC;
        ";

        return await connection.QueryAsync<DiagnosisResponse>(
            sql,
            new
            {
                HospitalId = hospitalId,
                DoctorId = doctorId,
                AppointmentId = appointmentId
            });
    }


    // ==========================================
    // GET DIAGNOSIS BY ID
    // ==========================================

    public async Task<DiagnosisResponse?> GetByIdAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid diagnosisId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"
            SELECT
                d.Id,
                d.AppointmentId,
                d.PatientId,

                CONCAT(
                    p.FirstName,
                    ' ',
                    COALESCE(p.LastName, '')
                ) AS PatientName,

                d.DoctorId,

                CONCAT(
                    doc.FirstName,
                    ' ',
                    COALESCE(doc.LastName, '')
                ) AS DoctorName,

                d.DiagnosisName,
                d.Description,
                d.Severity,
                d.Notes,
                d.DiagnosedAt,
                d.IsActive

            FROM Diagnoses d

            INNER JOIN Patients p
                ON p.Id = d.PatientId

            INNER JOIN Doctors doc
                ON doc.Id = d.DoctorId

            WHERE d.Id = @DiagnosisId
              AND d.HospitalId = @HospitalId
              AND d.DoctorId = @DoctorId
              AND d.IsActive = TRUE

            LIMIT 1;
        ";

        return await connection.QueryFirstOrDefaultAsync<DiagnosisResponse>(
            sql,
            new
            {
                HospitalId = hospitalId,
                DoctorId = doctorId,
                DiagnosisId = diagnosisId
            });
    }


    // ==========================================
    // CREATE DIAGNOSIS
    // ==========================================

    public async Task<DiagnosisResponse> CreateAsync(
        Guid hospitalId,
        Guid doctorId,
        CreateDiagnosisRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();

        var diagnosisId = Guid.NewGuid();

        // First get patient from the appointment.
        const string appointmentSql = @"
            SELECT PatientId
            FROM Appointments
            WHERE Id = @AppointmentId
              AND HospitalId = @HospitalId
              AND DoctorId = @DoctorId
            LIMIT 1;
        ";

        var patientId = await connection.QueryFirstOrDefaultAsync<Guid?>(
            appointmentSql,
            new
            {
                AppointmentId = request.AppointmentId,
                HospitalId = hospitalId,
                DoctorId = doctorId
            });

        if (patientId == null || patientId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "Appointment not found or does not belong to the logged-in doctor.");
        }

        const string insertSql = @"
            INSERT INTO Diagnoses
            (
                Id,
                AppointmentId,
                PatientId,
                HospitalId,
                DoctorId,
                DiagnosisName,
                Description,
                Severity,
                Notes,
                DiagnosedAt,
                IsActive,
                CreatedAt
            )
            VALUES
            (
                @Id,
                @AppointmentId,
                @PatientId,
                @HospitalId,
                @DoctorId,
                @DiagnosisName,
                @Description,
                @Severity,
                @Notes,
                CURRENT_TIMESTAMP,
                TRUE,
                CURRENT_TIMESTAMP
            );
        ";

        await connection.ExecuteAsync(
            insertSql,
            new
            {
                Id = diagnosisId,
                AppointmentId = request.AppointmentId,
                PatientId = patientId.Value,
                HospitalId = hospitalId,
                DoctorId = doctorId,
                request.DiagnosisName,
                request.Description,
                request.Severity,
                request.Notes
            });

        const string selectSql = @"
            SELECT
                d.Id,
                d.AppointmentId,
                d.PatientId,

                CONCAT(
                    p.FirstName,
                    ' ',
                    COALESCE(p.LastName, '')
                ) AS PatientName,

                d.DoctorId,

                CONCAT(
                    doc.FirstName,
                    ' ',
                    COALESCE(doc.LastName, '')
                ) AS DoctorName,

                d.DiagnosisName,
                d.Description,
                d.Severity,
                d.Notes,
                d.DiagnosedAt,
                d.IsActive

            FROM Diagnoses d

            INNER JOIN Patients p
                ON p.Id = d.PatientId

            INNER JOIN Doctors doc
                ON doc.Id = d.DoctorId

            WHERE d.Id = @DiagnosisId
              AND d.HospitalId = @HospitalId
              AND d.IsActive = TRUE

            LIMIT 1;
        ";

        return await connection.QuerySingleAsync<DiagnosisResponse>(
            selectSql,
            new
            {
                DiagnosisId = diagnosisId,
                HospitalId = hospitalId
            });
    }


    // ==========================================
    // UPDATE DIAGNOSIS
    // ==========================================

    public async Task<bool> UpdateAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid diagnosisId,
        UpdateDiagnosisRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"
            UPDATE Diagnoses

            SET
                DiagnosisName = @DiagnosisName,
                Description = @Description,
                Severity = @Severity,
                Notes = @Notes,
                UpdatedAt = CURRENT_TIMESTAMP

            WHERE Id = @DiagnosisId
              AND HospitalId = @HospitalId
              AND DoctorId = @DoctorId
              AND IsActive = TRUE;
        ";

        var rowsAffected = await connection.ExecuteAsync(
            sql,
            new
            {
                DiagnosisId = diagnosisId,
                HospitalId = hospitalId,
                DoctorId = doctorId,
                request.DiagnosisName,
                request.Description,
                request.Severity,
                request.Notes
            });

        return rowsAffected > 0;
    }


    // ==========================================
    // DELETE DIAGNOSIS
    // ==========================================

    public async Task<bool> DeleteAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid diagnosisId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"
            UPDATE Diagnoses

            SET
                IsActive = FALSE,
                UpdatedAt = CURRENT_TIMESTAMP

            WHERE Id = @DiagnosisId
              AND HospitalId = @HospitalId
              AND DoctorId = @DoctorId
              AND IsActive = TRUE;
        ";

        var rowsAffected = await connection.ExecuteAsync(
            sql,
            new
            {
                DiagnosisId = diagnosisId,
                HospitalId = hospitalId,
                DoctorId = doctorId
            });

        return rowsAffected > 0;
    }
}