using Dapper;
using Healtive.Application.DTOs.Doctor.Consultation;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.Doctors;

public class ConsultationRepository : IConsultationRepository
{
    private readonly IDbConnectionFactory _db;

    public ConsultationRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    // =========================================================
    // GET APPOINTMENT
    // Used before creating consultation
    // =========================================================

    public async Task<Consultation?>
        GetAppointmentForConsultationAsync(
            Guid hospitalId,
            Guid doctorId,
            Guid appointmentId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    a.Id,
    a.HospitalId,
    a.PatientId,
    a.DoctorId,
    a.AppointmentDate
FROM Appointments a
WHERE a.Id = @AppointmentId
AND a.HospitalId = @HospitalId
AND a.DoctorId = @DoctorId;";

        return await connection.QueryFirstOrDefaultAsync<Consultation>(
            sql,
            new
            {
                AppointmentId = appointmentId,
                HospitalId = hospitalId,
                DoctorId = doctorId
            });
    }

    // =========================================================
    // CHECK DUPLICATE CONSULTATION
    // =========================================================

    public async Task<bool>
        ExistsByAppointmentIdAsync(
            Guid appointmentId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Consultations
WHERE AppointmentId = @AppointmentId;";

        var count =
            await connection.ExecuteScalarAsync<int>(
                sql,
                new
                {
                    AppointmentId = appointmentId
                });

        return count > 0;
    }

    // =========================================================
    // CREATE
    // =========================================================

    public async Task CreateAsync(
        Consultation consultation)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
INSERT INTO Consultations
(
    Id,
    HospitalId,
    AppointmentId,
    PatientId,
    DoctorId,
    ConsultationDate,
    ChiefComplaint,
    ClinicalNotes,
    ExaminationNotes,
    TreatmentNotes,
    Advice,
    IsCompleted,
    CompletedAt,
    CreatedAt
)
VALUES
(
    @Id,
    @HospitalId,
    @AppointmentId,
    @PatientId,
    @DoctorId,
    @ConsultationDate,
    @ChiefComplaint,
    @ClinicalNotes,
    @ExaminationNotes,
    @TreatmentNotes,
    @Advice,
    @IsCompleted,
    @CompletedAt,
    @CreatedAt
);";

        await connection.ExecuteAsync(
            sql,
            consultation);
    }

    // =========================================================
    // GET BY APPOINTMENT
    // =========================================================

    public async Task<ConsultationResponse?>
        GetByAppointmentIdAsync(
            Guid hospitalId,
            Guid doctorId,
            Guid appointmentId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    Id,
    HospitalId,
    AppointmentId,
    PatientId,
    DoctorId,
    ConsultationDate,
    ChiefComplaint,
    ClinicalNotes,
    ExaminationNotes,
    TreatmentNotes,
    Advice,
    IsCompleted,
    CompletedAt,
    CreatedAt,
    UpdatedAt

FROM Consultations

WHERE HospitalId = @HospitalId
AND DoctorId = @DoctorId
AND AppointmentId = @AppointmentId;";

        return await connection.QueryFirstOrDefaultAsync<
            ConsultationResponse>(
                sql,
                new
                {
                    HospitalId = hospitalId,
                    DoctorId = doctorId,
                    AppointmentId = appointmentId
                });
    }

    // =========================================================
    // GET ENTITY BY ID
    // =========================================================

    public async Task<Consultation?>
        GetEntityByIdAsync(
            Guid hospitalId,
            Guid doctorId,
            Guid consultationId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    Id,
    HospitalId,
    AppointmentId,
    PatientId,
    DoctorId,
    ConsultationDate,
    ChiefComplaint,
    ClinicalNotes,
    ExaminationNotes,
    TreatmentNotes,
    Advice,
    IsCompleted,
    CompletedAt,
    CreatedAt,
    UpdatedAt

FROM Consultations

WHERE Id = @ConsultationId
AND HospitalId = @HospitalId
AND DoctorId = @DoctorId;";

        return await connection.QueryFirstOrDefaultAsync<Consultation>(
            sql,
            new
            {
                ConsultationId = consultationId,
                HospitalId = hospitalId,
                DoctorId = doctorId
            });
    }

    // =========================================================
    // UPDATE
    // =========================================================

    public async Task UpdateAsync(
        Consultation consultation)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Consultations

SET
    ChiefComplaint = @ChiefComplaint,
    ClinicalNotes = @ClinicalNotes,
    ExaminationNotes = @ExaminationNotes,
    TreatmentNotes = @TreatmentNotes,
    Advice = @Advice,
    UpdatedAt = @UpdatedAt

WHERE Id = @Id
AND HospitalId = @HospitalId
AND DoctorId = @DoctorId;";

        await connection.ExecuteAsync(
            sql,
            consultation);
    }

    // =========================================================
    // GET APPOINTMENT STATUS
    // =========================================================

    public async Task<string?>
        GetAppointmentStatusCodeAsync(
            Guid hospitalId,
            Guid doctorId,
            Guid appointmentId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT s.Code
FROM Appointments a
INNER JOIN AppointmentStatuses s
    ON s.Id = a.AppointmentStatusId
WHERE a.Id = @AppointmentId
AND a.HospitalId = @HospitalId
AND a.DoctorId = @DoctorId
LIMIT 1;";

        return await connection.QueryFirstOrDefaultAsync<string?>(
            sql,
            new
            {
                AppointmentId = appointmentId,
                HospitalId = hospitalId,
                DoctorId = doctorId
            });
    }

    // =========================================================
    // COMPLETE CONSULTATION
    // =========================================================

    public async Task<ConsultationResponse?>
        CompleteConsultationAsync(
            Guid hospitalId,
            Guid doctorId,
            Guid consultationId,
            Guid changedByUserId)
    {
        using var connection = _db.CreateConnection();

        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            // =====================================================
            // 1. RESOLVE COMPLETED APPOINTMENT STATUS
            // =====================================================

            const string statusSql = @"
SELECT Id
FROM AppointmentStatuses
WHERE Code = 'COMPLETED'
LIMIT 1;";

            var completedStatusId =
                await connection.QueryFirstOrDefaultAsync<Guid?>(
                    statusSql,
                    transaction: transaction);

            if (completedStatusId == null ||
                completedStatusId == Guid.Empty)
            {
                transaction.Rollback();

                return null;
            }

            // =====================================================
            // 2. VERIFY CONSULTATION OWNERSHIP
            // =====================================================

            const string consultationSql = @"
SELECT
    Id,
    AppointmentId
FROM Consultations
WHERE Id = @ConsultationId
AND HospitalId = @HospitalId
AND DoctorId = @DoctorId
LIMIT 1;";

            var consultation =
                await connection.QueryFirstOrDefaultAsync<ConsultationCompletionDbModel>(
                    consultationSql,
                    new
                    {
                        ConsultationId = consultationId,
                        HospitalId = hospitalId,
                        DoctorId = doctorId
                    },
                    transaction);

            if (consultation == null)
            {
                transaction.Rollback();

                return null;
            }

            // =====================================================
            // 3. VERIFY APPOINTMENT OWNERSHIP
            // =====================================================

            const string appointmentSql = @"
SELECT Id
FROM Appointments
WHERE Id = @AppointmentId
AND HospitalId = @HospitalId
AND DoctorId = @DoctorId
LIMIT 1;";

            var appointmentId =
                await connection.QueryFirstOrDefaultAsync<Guid?>(
                    appointmentSql,
                    new
                    {
                        AppointmentId = consultation.AppointmentId,
                        HospitalId = hospitalId,
                        DoctorId = doctorId
                    },
                    transaction);

            if (appointmentId == null ||
                appointmentId == Guid.Empty)
            {
                transaction.Rollback();

                return null;
            }

            // =====================================================
            // 4. UPDATE CONSULTATION
            // =====================================================

            const string updateConsultationSql = @"
UPDATE Consultations

SET
    IsCompleted = TRUE,
    CompletedAt = UTC_TIMESTAMP(),
    UpdatedAt = UTC_TIMESTAMP()

WHERE Id = @ConsultationId
AND HospitalId = @HospitalId
AND DoctorId = @DoctorId;";

            await connection.ExecuteAsync(
                updateConsultationSql,
                new
                {
                    ConsultationId = consultationId,
                    HospitalId = hospitalId,
                    DoctorId = doctorId
                },
                transaction);

            // =====================================================
            // 5. UPDATE APPOINTMENT STATUS
            // =====================================================

            const string updateAppointmentSql = @"
UPDATE Appointments

SET
    AppointmentStatusId = @CompletedStatusId,
    UpdatedAt = UTC_TIMESTAMP()

WHERE Id = @AppointmentId
AND HospitalId = @HospitalId
AND DoctorId = @DoctorId;";

            await connection.ExecuteAsync(
                updateAppointmentSql,
                new
                {
                    AppointmentId = consultation.AppointmentId,
                    HospitalId = hospitalId,
                    DoctorId = doctorId,
                    CompletedStatusId = completedStatusId
                },
                transaction);

            // =====================================================
            // 6. INSERT APPOINTMENT HISTORY
            // =====================================================

            const string insertHistorySql = @"
INSERT INTO AppointmentHistory
(
    Id,
    AppointmentId,
    AppointmentStatusId,
    ChangedByUserId,
    Remarks,
    ChangedAt
)
VALUES
(
    @Id,
    @AppointmentId,
    @AppointmentStatusId,
    @ChangedByUserId,
    @Remarks,
    @ChangedAt
);";

            await connection.ExecuteAsync(
                insertHistorySql,
                new
                {
                    Id = Guid.NewGuid(),
                    AppointmentId = consultation.AppointmentId,
                    AppointmentStatusId = completedStatusId,
                    ChangedByUserId = changedByUserId,
                    Remarks = "Consultation completed.",
                    ChangedAt = DateTime.UtcNow
                },
                transaction);

            // =====================================================
            // 7. COMMIT
            // =====================================================

            transaction.Commit();

            return await GetByAppointmentIdAsync(
                hospitalId,
                doctorId,
                consultation.AppointmentId);
        }
        catch
        {
            transaction.Rollback();

            throw;
        }
    }

    // =========================================================
    // PRIVATE DB MODELS
    // =========================================================

    private class ConsultationCompletionDbModel
    {
        public Guid Id { get; set; }

        public Guid AppointmentId { get; set; }
    }
}