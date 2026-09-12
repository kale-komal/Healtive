using Dapper;
using Healtive.Application.DTOs.Doctor.MedicalHistory;
using Healtive.Application.Interfaces.Repositories;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.Doctors;

public class MedicalHistoryRepository : IMedicalHistoryRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public MedicalHistoryRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    // ==========================================
    // GET ALL MEDICAL HISTORY FOR PATIENT
    // ==========================================

    public async Task<IEnumerable<MedicalHistoryResponse>> GetByPatientIdAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid patientId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"
            SELECT
                pmh.Id,
                pmh.PatientId,

                CONCAT(
                    p.FirstName,
                    ' ',
                    COALESCE(p.LastName, '')
                ) AS PatientName,

                pmh.MedicalCondition,
                pmh.Diagnosis,
                pmh.Treatment,
                pmh.Notes,
                pmh.RecordedAt,

                pmh.DoctorId AS RecordedByDoctorId,

                d.FullName AS RecordedByDoctorName

            FROM PatientMedicalHistories pmh

            INNER JOIN Patients p
                ON p.Id = pmh.PatientId

            INNER JOIN Doctors d
                ON d.Id = pmh.DoctorId

            WHERE pmh.PatientId = @PatientId
              AND pmh.HospitalId = @HospitalId
              AND pmh.DoctorId = @DoctorId
              AND pmh.IsDeleted = FALSE

            ORDER BY pmh.RecordedAt DESC;
        ";

        return await connection.QueryAsync<MedicalHistoryResponse>(
            sql,
            new
            {
                HospitalId = hospitalId,
                DoctorId = doctorId,
                PatientId = patientId
            });
    }


    // ==========================================
    // GET MEDICAL HISTORY BY ID
    // ==========================================

    public async Task<MedicalHistoryResponse?> GetByIdAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid historyId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"
            SELECT
                pmh.Id,
                pmh.PatientId,

                CONCAT(
                    p.FirstName,
                    ' ',
                    COALESCE(p.LastName, '')
                ) AS PatientName,

                pmh.MedicalCondition,
                pmh.Diagnosis,
                pmh.Treatment,
                pmh.Notes,
                pmh.RecordedAt,

                pmh.DoctorId AS RecordedByDoctorId,

                d.FullName AS RecordedByDoctorName

            FROM PatientMedicalHistories pmh

            INNER JOIN Patients p
                ON p.Id = pmh.PatientId

            INNER JOIN Doctors d
                ON d.Id = pmh.DoctorId

            WHERE pmh.Id = @HistoryId
              AND pmh.HospitalId = @HospitalId
              AND pmh.DoctorId = @DoctorId
              AND pmh.IsDeleted = FALSE

            LIMIT 1;
        ";

        return await connection.QueryFirstOrDefaultAsync<MedicalHistoryResponse>(
            sql,
            new
            {
                HospitalId = hospitalId,
                DoctorId = doctorId,
                HistoryId = historyId
            });
    }


    // ==========================================
    // CREATE MEDICAL HISTORY
    // ==========================================

    public async Task<MedicalHistoryResponse> CreateAsync(
        Guid hospitalId,
        Guid doctorId,
        CreateMedicalHistoryRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();

        var historyId = Guid.NewGuid();

        const string insertSql = @"
            INSERT INTO PatientMedicalHistories
            (
                Id,
                PatientId,
                HospitalId,
                DoctorId,
                MedicalCondition,
                Diagnosis,
                Treatment,
                Notes,
                RecordedAt,
                IsDeleted
            )
            VALUES
            (
                @Id,
                @PatientId,
                @HospitalId,
                @DoctorId,
                @MedicalCondition,
                @Diagnosis,
                @Treatment,
                @Notes,
                CURRENT_TIMESTAMP,
                FALSE
            );
        ";

        await connection.ExecuteAsync(
            insertSql,
            new
            {
                Id = historyId,
                PatientId = request.PatientId,
                HospitalId = hospitalId,
                DoctorId = doctorId,
                request.MedicalCondition,
                request.Diagnosis,
                request.Treatment,
                request.Notes
            });

        const string selectSql = @"
            SELECT
                pmh.Id,
                pmh.PatientId,

                CONCAT(
                    p.FirstName,
                    ' ',
                    COALESCE(p.LastName, '')
                ) AS PatientName,

                pmh.MedicalCondition,
                pmh.Diagnosis,
                pmh.Treatment,
                pmh.Notes,
                pmh.RecordedAt,

                pmh.DoctorId AS RecordedByDoctorId,

                d.FullName AS RecordedByDoctorName

            FROM PatientMedicalHistories pmh

            INNER JOIN Patients p
                ON p.Id = pmh.PatientId

            INNER JOIN Doctors d
                ON d.Id = pmh.DoctorId

            WHERE pmh.Id = @Id
              AND pmh.HospitalId = @HospitalId
              AND pmh.IsDeleted = FALSE

            LIMIT 1;
        ";

        return await connection.QuerySingleAsync<MedicalHistoryResponse>(
            selectSql,
            new
            {
                Id = historyId,
                HospitalId = hospitalId
            });
    }


    // ==========================================
    // UPDATE MEDICAL HISTORY
    // ==========================================

    public async Task<bool> UpdateAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid historyId,
        UpdateMedicalHistoryRequest request)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"
            UPDATE PatientMedicalHistories

            SET
                MedicalCondition = @MedicalCondition,
                Diagnosis = @Diagnosis,
                Treatment = @Treatment,
                Notes = @Notes,
                UpdatedAt = CURRENT_TIMESTAMP

            WHERE Id = @HistoryId
              AND HospitalId = @HospitalId
              AND DoctorId = @DoctorId
              AND IsDeleted = FALSE;
        ";

        var rowsAffected = await connection.ExecuteAsync(
            sql,
            new
            {
                HistoryId = historyId,
                HospitalId = hospitalId,
                DoctorId = doctorId,
                request.MedicalCondition,
                request.Diagnosis,
                request.Treatment,
                request.Notes
            });

        return rowsAffected > 0;
    }


    // ==========================================
    // DELETE MEDICAL HISTORY
    // ==========================================

    public async Task<bool> DeleteAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid historyId)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql = @"
            UPDATE PatientMedicalHistories

            SET
                IsDeleted = TRUE,
                UpdatedAt = CURRENT_TIMESTAMP

            WHERE Id = @HistoryId
              AND HospitalId = @HospitalId
              AND DoctorId = @DoctorId
              AND IsDeleted = FALSE;
        ";

        var rowsAffected = await connection.ExecuteAsync(
            sql,
            new
            {
                HistoryId = historyId,
                HospitalId = hospitalId,
                DoctorId = doctorId
            });

        return rowsAffected > 0;
    }
}