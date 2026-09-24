using Dapper;
using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Doctor.FollowUp;
using Healtive.Application.Interfaces.Repositories;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.Doctors;

public class DoctorFollowUpRepository : IDoctorFollowUpRepository
{
    private readonly IDbConnectionFactory _db;

    public DoctorFollowUpRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    // =========================================================
    // GET UPCOMING FOLLOW-UPS
    // =========================================================

    public async Task<IEnumerable<FollowUpResponse>> GetUpcomingAsync(
        Guid hospitalId,
        Guid doctorId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    fu.Id,
    fu.HospitalId,
    fu.BranchId,
    fu.AppointmentId,
    fu.PatientId,
    fu.DoctorId,
    fu.FollowUpDate,
    fu.FollowUpNotes,
    fu.Status,
    fu.IsCompleted,
    fu.CompletedAt,
    fu.CreatedAt,
    fu.UpdatedAt,

    CONCAT(
        p.FirstName,
        ' ',
        COALESCE(p.LastName, '')
    ) AS PatientName,

    doc.FullName AS DoctorName

FROM FollowUps fu

INNER JOIN Patients p
    ON p.Id = fu.PatientId

INNER JOIN Doctors doc
    ON doc.Id = fu.DoctorId

WHERE fu.HospitalId = @HospitalId
AND fu.DoctorId = @DoctorId
AND fu.FollowUpDate >= CURDATE()
AND fu.Status NOT IN ('Cancelled', 'Completed')

ORDER BY fu.FollowUpDate ASC, fu.CreatedAt ASC;";

        return await connection.QueryAsync<FollowUpResponse>(
            sql,
            new
            {
                HospitalId = hospitalId,
                DoctorId = doctorId
            });
    }

    // =========================================================
    // GET FOLLOW-UPS BY APPOINTMENT
    // =========================================================

    public async Task<IEnumerable<FollowUpResponse>> GetByAppointmentIdAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid appointmentId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    fu.Id,
    fu.HospitalId,
    fu.BranchId,
    fu.AppointmentId,
    fu.PatientId,
    fu.DoctorId,
    fu.FollowUpDate,
    fu.FollowUpNotes,
    fu.Status,
    fu.IsCompleted,
    fu.CompletedAt,
    fu.CreatedAt,
    fu.UpdatedAt,

    CONCAT(
        p.FirstName,
        ' ',
        COALESCE(p.LastName, '')
    ) AS PatientName,

    doc.FullName AS DoctorName

FROM FollowUps fu

INNER JOIN Patients p
    ON p.Id = fu.PatientId

INNER JOIN Doctors doc
    ON doc.Id = fu.DoctorId

WHERE fu.HospitalId = @HospitalId
AND fu.DoctorId = @DoctorId
AND fu.AppointmentId = @AppointmentId

ORDER BY fu.FollowUpDate DESC, fu.CreatedAt DESC;";

        return await connection.QueryAsync<FollowUpResponse>(
            sql,
            new
            {
                HospitalId = hospitalId,
                DoctorId = doctorId,
                AppointmentId = appointmentId
            });
    }

    // =========================================================
    // GET FOLLOW-UP BY ID
    // =========================================================

    public async Task<FollowUpResponse?> GetByIdAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid followUpId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    fu.Id,
    fu.HospitalId,
    fu.BranchId,
    fu.AppointmentId,
    fu.PatientId,
    fu.DoctorId,
    fu.FollowUpDate,
    fu.FollowUpNotes,
    fu.Status,
    fu.IsCompleted,
    fu.CompletedAt,
    fu.CreatedAt,
    fu.UpdatedAt,

    CONCAT(
        p.FirstName,
        ' ',
        COALESCE(p.LastName, '')
    ) AS PatientName,

    doc.FullName AS DoctorName

FROM FollowUps fu

INNER JOIN Patients p
    ON p.Id = fu.PatientId

INNER JOIN Doctors doc
    ON doc.Id = fu.DoctorId

WHERE fu.Id = @FollowUpId
AND fu.HospitalId = @HospitalId
AND fu.DoctorId = @DoctorId

LIMIT 1;";

        return await connection.QueryFirstOrDefaultAsync<FollowUpResponse>(
            sql,
            new
            {
                FollowUpId = followUpId,
                HospitalId = hospitalId,
                DoctorId = doctorId
            });
    }

    // =========================================================
    // CREATE FOLLOW-UP
    // =========================================================

    public async Task<ApiResponse<FollowUpResponse>> CreateAsync(
        Guid hospitalId,
        Guid doctorId,
        CreateFollowUpRequest request)
    {
        using var connection = _db.CreateConnection();

        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            // =====================================================
            // 1. VERIFY APPOINTMENT OWNERSHIP
            // =====================================================

            const string appointmentSql = @"
SELECT
    PatientId,
    BranchId

FROM Appointments

WHERE Id = @AppointmentId
AND HospitalId = @HospitalId
AND DoctorId = @DoctorId

LIMIT 1;";

            var appointment =
                await connection.QueryFirstOrDefaultAsync<AppointmentOwnershipDbModel>(
                    appointmentSql,
                    new
                    {
                        AppointmentId = request.AppointmentId,
                        HospitalId = hospitalId,
                        DoctorId = doctorId
                    },
                    transaction);

            if (appointment == null)
            {
                transaction.Rollback();

                return ApiResponse<FollowUpResponse>
                    .FailureResponse("Appointment not found or does not belong to the logged-in doctor.");
            }

            // =====================================================
            // 2. VALIDATE FOLLOW-UP DATE
            // =====================================================

            if (request.FollowUpDate < DateOnly.FromDateTime(DateTime.Today))
            {
                transaction.Rollback();

                return ApiResponse<FollowUpResponse>
                    .FailureResponse("Follow-up date cannot be in the past.");
            }

            // =====================================================
            // 3. INSERT FOLLOW-UP
            // =====================================================

            var followUpId = Guid.NewGuid();

            const string insertSql = @"
INSERT INTO FollowUps
(
    Id,
    HospitalId,
    BranchId,
    AppointmentId,
    PatientId,
    DoctorId,
    FollowUpDate,
    FollowUpNotes,
    Status,
    IsCompleted,
    CompletedAt,
    CreatedAt
)
VALUES
(
    @Id,
    @HospitalId,
    @BranchId,
    @AppointmentId,
    @PatientId,
    @DoctorId,
    @FollowUpDate,
    @FollowUpNotes,
    'Pending',
    FALSE,
    NULL,
    CURRENT_TIMESTAMP
);";

            await connection.ExecuteAsync(
                insertSql,
                new
                {
                    Id = followUpId,
                    HospitalId = hospitalId,
                    BranchId = appointment.BranchId,
                    AppointmentId = request.AppointmentId,
                    PatientId = appointment.PatientId,
                    DoctorId = doctorId,
                    request.FollowUpDate,
                    request.FollowUpNotes
                },
                transaction);

            // =====================================================
            // 4. COMMIT
            // =====================================================

            transaction.Commit();

            var created = await GetByIdAsync(
                hospitalId,
                doctorId,
                followUpId);

            return ApiResponse<FollowUpResponse>
                .SuccessResponse(
                    created!,
                    "Follow-up created successfully.");
        }
        catch
        {
            transaction.Rollback();

            throw;
        }
    }

    // =========================================================
    // UPDATE FOLLOW-UP
    // =========================================================

    public async Task<ApiResponse<FollowUpResponse>> UpdateAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid followUpId,
        UpdateFollowUpRequest request)
    {
        using var connection = _db.CreateConnection();

        var existing = await GetByIdAsync(
            hospitalId,
            doctorId,
            followUpId);

        if (existing == null)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Follow-up not found.");
        }

        // =====================================================
        // COMPLETED OR CANCELLED FOLLOW-UPS ARE READ-ONLY
        // =====================================================

        if (existing.Status == "Completed")
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Completed follow-up cannot be modified.");
        }

        if (existing.Status == "Cancelled")
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Cancelled follow-up cannot be modified.");
        }

        // =====================================================
        // VALIDATE STATUS
        // =====================================================

        if (request.Status != "Pending"
            && request.Status != "Completed"
            && request.Status != "Cancelled")
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Invalid follow-up status.");
        }

        // =====================================================
        // VALIDATE FOLLOW-UP DATE
        // =====================================================

        if (request.FollowUpDate < DateOnly.FromDateTime(DateTime.Today))
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Follow-up date cannot be in the past.");
        }

        // =====================================================
        // KEEP IsCompleted / CompletedAt CONSISTENT WITH STATUS
        // =====================================================

        var isCompleted = request.Status == "Completed";

        var completedAt = isCompleted
            ? (DateTime?)DateTime.UtcNow
            : null;

        const string sql = @"
UPDATE FollowUps

SET
    FollowUpDate = @FollowUpDate,
    FollowUpNotes = @FollowUpNotes,
    Status = @Status,
    IsCompleted = @IsCompleted,
    CompletedAt = @CompletedAt,
    UpdatedAt = CURRENT_TIMESTAMP

WHERE Id = @FollowUpId
AND HospitalId = @HospitalId
AND DoctorId = @DoctorId;";

        var rowsAffected = await connection.ExecuteAsync(
            sql,
            new
            {
                FollowUpId = followUpId,
                HospitalId = hospitalId,
                DoctorId = doctorId,
                request.FollowUpDate,
                request.FollowUpNotes,
                request.Status,
                IsCompleted = isCompleted,
                CompletedAt = completedAt
            });

        if (rowsAffected == 0)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Unable to update follow-up.");
        }

        var updated = await GetByIdAsync(
            hospitalId,
            doctorId,
            followUpId);

        return ApiResponse<FollowUpResponse>
            .SuccessResponse(
                updated!,
                "Follow-up updated successfully.");
    }

    // =========================================================
    // COMPLETE FOLLOW-UP
    // =========================================================

    public async Task<ApiResponse<FollowUpResponse>> CompleteAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid followUpId)
    {
        using var connection = _db.CreateConnection();

        var existing = await GetByIdAsync(
            hospitalId,
            doctorId,
            followUpId);

        if (existing == null)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Follow-up not found.");
        }

        // =====================================================
        // ALREADY COMPLETED
        // =====================================================

        if (existing.Status == "Completed")
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Follow-up is already completed.");
        }

        // =====================================================
        // CANCELLED FOLLOW-UPS CANNOT BE COMPLETED
        // =====================================================

        if (existing.Status == "Cancelled")
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Cancelled follow-up cannot be completed.");
        }

        const string sql = @"
UPDATE FollowUps

SET
    Status = 'Completed',
    IsCompleted = TRUE,
    CompletedAt = CURRENT_TIMESTAMP,
    UpdatedAt = CURRENT_TIMESTAMP

WHERE Id = @FollowUpId
AND HospitalId = @HospitalId
AND DoctorId = @DoctorId
AND Status = 'Pending';";

        var rowsAffected = await connection.ExecuteAsync(
            sql,
            new
            {
                FollowUpId = followUpId,
                HospitalId = hospitalId,
                DoctorId = doctorId
            });

        if (rowsAffected == 0)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Unable to complete follow-up.");
        }

        var completed = await GetByIdAsync(
            hospitalId,
            doctorId,
            followUpId);

        return ApiResponse<FollowUpResponse>
            .SuccessResponse(
                completed!,
                "Follow-up completed successfully.");
    }

    // =========================================================
    // CANCEL FOLLOW-UP
    // =========================================================

    public async Task<ApiResponse<FollowUpResponse>> CancelAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid followUpId)
    {
        using var connection = _db.CreateConnection();

        var existing = await GetByIdAsync(
            hospitalId,
            doctorId,
            followUpId);

        if (existing == null)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Follow-up not found.");
        }

        // =====================================================
        // COMPLETED FOLLOW-UPS CANNOT BE CANCELLED
        // =====================================================

        if (existing.Status == "Completed")
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Completed follow-up cannot be cancelled.");
        }

        // =====================================================
        // ALREADY CANCELLED
        // =====================================================

        if (existing.Status == "Cancelled")
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Follow-up is already cancelled.");
        }

        const string sql = @"
UPDATE FollowUps

SET
    Status = 'Cancelled',
    IsCompleted = FALSE,
    CompletedAt = NULL,
    UpdatedAt = CURRENT_TIMESTAMP

WHERE Id = @FollowUpId
AND HospitalId = @HospitalId
AND DoctorId = @DoctorId
AND Status = 'Pending';";

        var rowsAffected = await connection.ExecuteAsync(
            sql,
            new
            {
                FollowUpId = followUpId,
                HospitalId = hospitalId,
                DoctorId = doctorId
            });

        if (rowsAffected == 0)
        {
            return ApiResponse<FollowUpResponse>
                .FailureResponse("Unable to cancel follow-up.");
        }

        var cancelled = await GetByIdAsync(
            hospitalId,
            doctorId,
            followUpId);

        return ApiResponse<FollowUpResponse>
            .SuccessResponse(
                cancelled!,
                "Follow-up cancelled successfully.");
    }

    private class AppointmentOwnershipDbModel
    {
        public Guid PatientId { get; set; }

        public Guid BranchId { get; set; }
    }
}