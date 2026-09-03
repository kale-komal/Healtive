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
    // COMPLETE CONSULTATION
    // =========================================================

    public async Task CompleteAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid consultationId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Consultations

SET
    IsCompleted = 1,
    CompletedAt = UTC_TIMESTAMP(),
    UpdatedAt = UTC_TIMESTAMP()

WHERE Id = @ConsultationId
AND HospitalId = @HospitalId
AND DoctorId = @DoctorId;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                ConsultationId = consultationId,
                HospitalId = hospitalId,
                DoctorId = doctorId
            });
    }
}