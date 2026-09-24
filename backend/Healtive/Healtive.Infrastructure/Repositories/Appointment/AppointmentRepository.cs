using Dapper;
using Healtive.Application.DTOs.Appointment;
using Healtive.Application.DTOs.Common;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.Appointments;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly IDbConnectionFactory _db;

    public AppointmentRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<bool> ExistsAsync(
        Guid hospitalId,
        Guid appointmentId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Appointments
WHERE Id = @AppointmentId
AND HospitalId = @HospitalId;";

        var count = await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                AppointmentId = appointmentId,
                HospitalId = hospitalId
            });

        return count > 0;
    }

    public async Task<bool> HasConflictAsync(
        Guid doctorId,
        DateOnly appointmentDate,
        TimeSpan appointmentTime,
        Guid? excludeAppointmentId = null)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Appointments
WHERE DoctorId = @DoctorId
AND AppointmentDate = @AppointmentDate
AND AppointmentTime = @AppointmentTime
AND AppointmentStatusId IN
(
    SELECT Id
    FROM AppointmentStatuses
    WHERE Code NOT IN ('CANCELLED', 'COMPLETED', 'NO_SHOW')
)
AND
(
    @ExcludeAppointmentId IS NULL
    OR Id <> @ExcludeAppointmentId
);";

        var count = await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                DoctorId = doctorId,
                AppointmentDate =
                    appointmentDate.ToDateTime(TimeOnly.MinValue),
                AppointmentTime =
                    appointmentTime,
                ExcludeAppointmentId = excludeAppointmentId
            });

        return count > 0;
    }

    public async Task CreateAsync(
        Appointment appointment)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
INSERT INTO Appointments
(
    Id,
    AppointmentNumber,
    HospitalId,
    BranchId,
    PatientId,
    DoctorId,
    DepartmentId,
    AppointmentStatusId,
    AppointmentDate,
    AppointmentTime,
    TokenNumber,
    ConsultationType,
    ReasonForVisit,
    Notes,
    IsFirstVisit,
    CreatedByUserId,
    CreatedAt
)
VALUES
(
    @Id,
    @AppointmentNumber,
    @HospitalId,
    @BranchId,
    @PatientId,
    @DoctorId,
    @DepartmentId,
    @AppointmentStatusId,
    @AppointmentDate,
    @AppointmentTime,
    @TokenNumber,
    @ConsultationType,
    @ReasonForVisit,
    @Notes,
    @IsFirstVisit,
    @CreatedByUserId,
    @CreatedAt
);";

        await connection.ExecuteAsync(
            sql,
            new
            {
                appointment.Id,
                appointment.AppointmentNumber,
                appointment.HospitalId,
                appointment.BranchId,
                appointment.PatientId,
                appointment.DoctorId,
                appointment.DepartmentId,
                appointment.AppointmentStatusId,

                AppointmentDate =
                    appointment.AppointmentDate
                        .ToDateTime(TimeOnly.MinValue),

                appointment.AppointmentTime,
                appointment.TokenNumber,
                appointment.ConsultationType,
                appointment.ReasonForVisit,
                appointment.Notes,
                appointment.IsFirstVisit,
                appointment.CreatedByUserId,
                appointment.CreatedAt
            });
    }

    public async Task UpdateAsync(
        Appointment appointment)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Appointments
SET
    BranchId = @BranchId,
    DoctorId = @DoctorId,
    DepartmentId = @DepartmentId,
    AppointmentDate = @AppointmentDate,
    AppointmentTime = @AppointmentTime,
    TokenNumber = @TokenNumber,
    ConsultationType = @ConsultationType,
    ReasonForVisit = @ReasonForVisit,
    Notes = @Notes,
    IsFirstVisit = @IsFirstVisit,
    UpdatedAt = @UpdatedAt
WHERE Id = @Id
AND HospitalId = @HospitalId;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                appointment.Id,
                appointment.HospitalId,
                appointment.BranchId,
                appointment.DoctorId,
                appointment.DepartmentId,

                AppointmentDate =
                    appointment.AppointmentDate
                        .ToDateTime(TimeOnly.MinValue),

                appointment.AppointmentTime,
                appointment.TokenNumber,
                appointment.ConsultationType,
                appointment.ReasonForVisit,
                appointment.Notes,
                appointment.IsFirstVisit,
                appointment.UpdatedAt
            });
    }

    public async Task<AppointmentResponse?> GetByIdAsync(
        Guid hospitalId,
        Guid appointmentId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    a.Id,
    a.AppointmentNumber,
    a.HospitalId,
    a.BranchId,
    a.PatientId,
    a.DoctorId,
    a.DepartmentId,
    a.AppointmentStatusId,
    s.Name AS AppointmentStatusName,
    a.AppointmentDate,
    a.AppointmentTime,
    a.TokenNumber,
    a.ConsultationType,
    a.ReasonForVisit,
    a.Notes,
    a.IsFirstVisit,
    a.CreatedAt,
    a.UpdatedAt
FROM Appointments a
INNER JOIN AppointmentStatuses s
    ON s.Id = a.AppointmentStatusId
WHERE a.Id = @AppointmentId
AND a.HospitalId = @HospitalId;";

        var row =
            await connection.QueryFirstOrDefaultAsync<AppointmentDbModel>(
                sql,
                new
                {
                    AppointmentId = appointmentId,
                    HospitalId = hospitalId
                });

        return row == null
            ? null
            : MapResponse(row);
    }

    public async Task<PagedResponse<AppointmentResponse>> GetAllAsync(
        Guid hospitalId,
        AppointmentFilterRequest request)
    {
        using var connection = _db.CreateConnection();

        var conditions = @"
WHERE a.HospitalId = @HospitalId";

        if (request.BranchId.HasValue)
        {
            conditions += @"
AND a.BranchId = @BranchId";
        }

        if (request.DoctorId.HasValue)
        {
            conditions += @"
AND a.DoctorId = @DoctorId";
        }

        if (request.PatientId.HasValue)
        {
            conditions += @"
AND a.PatientId = @PatientId";
        }

        if (request.DepartmentId.HasValue)
        {
            conditions += @"
AND a.DepartmentId = @DepartmentId";
        }

        if (request.AppointmentStatusId.HasValue)
        {
            conditions += @"
AND a.AppointmentStatusId = @AppointmentStatusId";
        }

        if (request.FromDate.HasValue)
        {
            conditions += @"
AND a.AppointmentDate >= @FromDate";
        }

        if (request.ToDate.HasValue)
        {
            conditions += @"
AND a.AppointmentDate <= @ToDate";
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            conditions += @"
AND
(
    a.AppointmentNumber LIKE @Search
    OR p.FirstName LIKE @Search
    OR p.LastName LIKE @Search
    OR d.FullName LIKE @Search
)";
        }

        var parameters = new
        {
            HospitalId = hospitalId,
            request.BranchId,
            request.DoctorId,
            request.PatientId,
            request.DepartmentId,
            request.AppointmentStatusId,

            FromDate = request.FromDate.HasValue
                ? request.FromDate.Value.ToDateTime(TimeOnly.MinValue)
                : (DateTime?)null,

            ToDate = request.ToDate.HasValue
                ? request.ToDate.Value.ToDateTime(TimeOnly.MinValue)
                : (DateTime?)null,

            Search = $"%{request.Search}%",

            PageSize = request.PageSize,
            Offset = (request.Page - 1) * request.PageSize
        };

        var countSql = $@"
SELECT COUNT(*)
FROM Appointments a
INNER JOIN Patients p
    ON p.Id = a.PatientId
INNER JOIN Doctors d
    ON d.Id = a.DoctorId
{conditions};";

        var totalCount =
            await connection.ExecuteScalarAsync<int>(
                countSql,
                parameters);

        var sql = $@"
SELECT
    a.Id,
    a.AppointmentNumber,
    a.HospitalId,
    a.BranchId,
    a.PatientId,
    a.DoctorId,
    a.DepartmentId,
    a.AppointmentStatusId,
    s.Name AS AppointmentStatusName,
    a.AppointmentDate,
    a.AppointmentTime,
    a.TokenNumber,
    a.ConsultationType,
    a.ReasonForVisit,
    a.Notes,
    a.IsFirstVisit,
    a.CreatedAt,
    a.UpdatedAt
FROM Appointments a
INNER JOIN AppointmentStatuses s
    ON s.Id = a.AppointmentStatusId
INNER JOIN Patients p
    ON p.Id = a.PatientId
INNER JOIN Doctors d
    ON d.Id = a.DoctorId
{conditions}
ORDER BY a.AppointmentDate DESC, a.AppointmentTime ASC
LIMIT @PageSize OFFSET @Offset;";

        var rows =
            await connection.QueryAsync<AppointmentDbModel>(
                sql,
                parameters);

        var items = rows.Select(MapResponse);

        var totalPages =
            request.PageSize == 0
                ? 0
                : (int)Math.Ceiling(
                    totalCount / (double)request.PageSize);

        return new PagedResponse<AppointmentResponse>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }

    public async Task<IEnumerable<DoctorAvailableSlotResponse>>
        GetDoctorAvailableSlotsAsync(
            Guid hospitalId,
            Guid doctorId,
            DateOnly appointmentDate)
    {
        using var connection = _db.CreateConnection();

        var dayOfWeek = (byte)appointmentDate.DayOfWeek;

        const string sql = @"
SELECT
    @AppointmentDate AS AppointmentDate,
    da.StartTime,
    da.EndTime,
    TRUE AS IsAvailable
FROM DoctorAvailability da
INNER JOIN Doctors d
    ON d.Id = da.DoctorId
WHERE da.DoctorId = @DoctorId
AND d.HospitalId = @HospitalId
AND da.DayOfWeek = @DayOfWeek
AND da.IsAvailable = 1
AND d.IsActive = 1
AND d.IsDeleted = 0
AND NOT EXISTS
(
    SELECT 1
    FROM DoctorLeaves dl
    WHERE dl.DoctorId = da.DoctorId
    AND @AppointmentDate BETWEEN dl.FromDate AND dl.ToDate
    AND dl.IsApproved = 1
);";

        var rows =
            await connection.QueryAsync<DoctorAvailableSlotDbModel>(
                sql,
                new
                {
                    HospitalId = hospitalId,
                    DoctorId = doctorId,
                    DayOfWeek = dayOfWeek,
                    AppointmentDate =
                        appointmentDate.ToDateTime(TimeOnly.MinValue)
                });

        var bookedTimes = await connection.QueryAsync<TimeSpan>(
            @"
SELECT AppointmentTime
FROM Appointments
WHERE DoctorId = @DoctorId
AND AppointmentDate = @AppointmentDate
AND AppointmentStatusId IN
(
    SELECT Id
    FROM AppointmentStatuses
    WHERE Code NOT IN ('CANCELLED', 'COMPLETED', 'NO_SHOW')
);",
            new
            {
                DoctorId = doctorId,
                AppointmentDate =
                    appointmentDate.ToDateTime(TimeOnly.MinValue)
            });

        var booked = bookedTimes.ToHashSet();

        return rows.Select(x => new DoctorAvailableSlotResponse
        {
            AppointmentDate = appointmentDate,
            StartTime = x.StartTime,
            EndTime = x.EndTime,
            IsAvailable = !booked.Contains(x.StartTime)
        });
    }

    public async Task DeleteAsync(
        Guid hospitalId,
        Guid appointmentId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
DELETE FROM Appointments
WHERE Id = @AppointmentId
AND HospitalId = @HospitalId;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                AppointmentId = appointmentId,
                HospitalId = hospitalId
            });
    }

    public async Task UpdateStatusAsync(
        Guid hospitalId,
        Guid appointmentId,
        Guid appointmentStatusId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Appointments
SET
    AppointmentStatusId = @AppointmentStatusId,
    UpdatedAt = @UpdatedAt
WHERE Id = @AppointmentId
AND HospitalId = @HospitalId;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                AppointmentId = appointmentId,
                HospitalId = hospitalId,
                AppointmentStatusId = appointmentStatusId,
                UpdatedAt = DateTime.UtcNow
            });
    }

    public async Task<AppointmentCheckInResult> CheckInAsync(
        Guid hospitalId,
        Guid appointmentId,
        Guid? doctorId,
        Guid changedByUserId,
        string remark)
    {
        using var connection = _db.CreateConnection();

        using var transaction = connection.BeginTransaction();

        const string appointmentSql = @"
SELECT
    a.Id,
    a.DoctorId,
    a.AppointmentDate,
    s.Code AS AppointmentStatusCode
FROM Appointments a
INNER JOIN AppointmentStatuses s
    ON s.Id = a.AppointmentStatusId
WHERE a.Id = @AppointmentId
AND a.HospitalId = @HospitalId
AND (@DoctorId IS NULL OR a.DoctorId = @DoctorId)
FOR UPDATE;";

        var appointment =
            await connection.QueryFirstOrDefaultAsync<CheckInAppointmentDbModel>(
                appointmentSql,
                new
                {
                    AppointmentId = appointmentId,
                    HospitalId = hospitalId,
                    DoctorId = doctorId
                },
                transaction);

        if (appointment == null)
        {
            transaction.Rollback();

            return new AppointmentCheckInResult
            {
                Outcome = AppointmentCheckInOutcome.NotFound
            };
        }

        if (appointment.AppointmentStatusCode == "CHECKED_IN")
        {
            transaction.Rollback();

            return new AppointmentCheckInResult
            {
                Outcome = AppointmentCheckInOutcome.AlreadyCheckedIn
            };
        }

        if (appointment.AppointmentStatusCode is
            "CANCELLED" or "NO_SHOW" or "COMPLETED")
        {
            transaction.Rollback();

            return new AppointmentCheckInResult
            {
                Outcome = AppointmentCheckInOutcome.TerminalStatus,
                AppointmentStatusCode = appointment.AppointmentStatusCode
            };
        }

        var lockName =
            $"healtive:check-in:{appointment.DoctorId}:{appointment.AppointmentDate:yyyy-MM-dd}";

        var lockAcquired =
            await connection.ExecuteScalarAsync<int>(
                "SELECT GET_LOCK(@LockName, 10);",
                new
                {
                    LockName = lockName
                },
                transaction);

        if (lockAcquired != 1)
        {
            transaction.Rollback();

            throw new InvalidOperationException(
                "Could not acquire check-in lock. Please try again.");
        }

        try
        {
            var checkedInStatusId =
                await connection.ExecuteScalarAsync<Guid>(
                    @"SELECT Id
                      FROM AppointmentStatuses
                      WHERE Code = 'CHECKED_IN'
                      AND IsActive = 1
                      LIMIT 1;",
                    transaction: transaction);

            if (checkedInStatusId == Guid.Empty)
            {
                transaction.Rollback();

                return new AppointmentCheckInResult
                {
                    Outcome = AppointmentCheckInOutcome.StatusNotConfigured
                };
            }

            var nextToken =
                await connection.ExecuteScalarAsync<int>(
                    @"SELECT COALESCE(MAX(TokenNumber), 0) + 1
                      FROM Appointments
                      WHERE DoctorId = @DoctorId
                      AND AppointmentDate = @AppointmentDate
                      FOR UPDATE;",
                    new
                    {
                        DoctorId = appointment.DoctorId,
                        AppointmentDate = appointment.AppointmentDate
                    },
                    transaction);

            var now = DateTime.UtcNow;

            await connection.ExecuteAsync(
                @"UPDATE Appointments
                  SET
                      AppointmentStatusId = @AppointmentStatusId,
                      TokenNumber = @TokenNumber,
                      UpdatedAt = @UpdatedAt
                  WHERE Id = @AppointmentId
                  AND HospitalId = @HospitalId;",
                new
                {
                    AppointmentStatusId = checkedInStatusId,
                    TokenNumber = nextToken,
                    UpdatedAt = now,
                    AppointmentId = appointmentId,
                    HospitalId = hospitalId
                },
                transaction);

            await connection.ExecuteAsync(
                @"INSERT INTO AppointmentHistory
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
                  );",
                new
                {
                    Id = Guid.NewGuid(),
                    AppointmentId = appointmentId,
                    AppointmentStatusId = checkedInStatusId,
                    ChangedByUserId = changedByUserId,
                    Remarks = remark,
                    ChangedAt = now
                },
                transaction);

            transaction.Commit();

            var response =
                await GetByIdAsync(
                    hospitalId,
                    appointmentId);

            return new AppointmentCheckInResult
            {
                Outcome = AppointmentCheckInOutcome.Success,
                Appointment = response
            };
        }
        finally
        {
            await connection.ExecuteAsync(
                "SELECT RELEASE_LOCK(@LockName);",
                new
                {
                    LockName = lockName
                });
        }
    }

    public async Task<IEnumerable<AppointmentQueueItemResponse>>
        GetQueueAsync(
            Guid hospitalId,
            Guid doctorId,
            DateOnly appointmentDate)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    a.Id AS AppointmentId,
    a.AppointmentNumber,
    a.PatientId,
    CONCAT(
        p.FirstName,
        ' ',
        p.LastName
    ) AS PatientName,
    p.PatientCode,
    a.DoctorId,
    d.FullName AS DoctorName,
    a.AppointmentDate,
    a.AppointmentTime,
    a.TokenNumber,
    s.Name AS AppointmentStatus,
    s.Code AS AppointmentStatusCode,
    a.ConsultationType,
    a.IsFirstVisit
FROM Appointments a
INNER JOIN AppointmentStatuses s
    ON s.Id = a.AppointmentStatusId
INNER JOIN Patients p
    ON p.Id = a.PatientId
INNER JOIN Doctors d
    ON d.Id = a.DoctorId
WHERE a.HospitalId = @HospitalId
AND a.DoctorId = @DoctorId
AND a.AppointmentDate = @AppointmentDate
AND s.Code IN ('CHECKED_IN', 'CALLED')
AND p.IsDeleted = 0
AND d.IsDeleted = 0
ORDER BY
    (a.TokenNumber IS NULL) ASC,
    a.TokenNumber ASC,
    a.AppointmentTime ASC;";

        return await connection.QueryAsync<AppointmentQueueItemResponse>(
            sql,
            new
            {
                HospitalId = hospitalId,
                DoctorId = doctorId,
                AppointmentDate =
                    appointmentDate.ToDateTime(TimeOnly.MinValue)
            });
    }

    public async Task<string?> GetUserNameAsync(
        Guid hospitalId,
        Guid userId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT CONCAT(FirstName, ' ', LastName)
FROM Users
WHERE Id = @UserId
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<string?>(
            sql,
            new
            {
                UserId = userId,
                HospitalId = hospitalId
            });
    }

    public async Task AddHistoryAsync(
        AppointmentHistory history)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
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
            sql,
            history);
    }

    public async Task<IEnumerable<AppointmentHistory>>
        GetHistoryAsync(
            Guid hospitalId,
            Guid appointmentId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    h.Id,
    h.AppointmentId,
    h.AppointmentStatusId,
    h.ChangedByUserId,
    h.Remarks,
    h.ChangedAt
FROM AppointmentHistory h
INNER JOIN Appointments a
    ON a.Id = h.AppointmentId
WHERE h.AppointmentId = @AppointmentId
AND a.HospitalId = @HospitalId
ORDER BY h.ChangedAt DESC;";

        return await connection.QueryAsync<AppointmentHistory>(
            sql,
            new
            {
                AppointmentId = appointmentId,
                HospitalId = hospitalId
            });
    }

    public async Task AddNoteAsync(
        AppointmentNote note)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
INSERT INTO AppointmentNotes
(
    Id,
    AppointmentId,
    Note,
    CreatedByUserId,
    CreatedAt
)
VALUES
(
    @Id,
    @AppointmentId,
    @Note,
    @CreatedByUserId,
    @CreatedAt
);";

        await connection.ExecuteAsync(
            sql,
            note);
    }

    public async Task AddAttachmentAsync(
        AppointmentAttachment attachment)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
INSERT INTO AppointmentAttachments
(
    Id,
    AppointmentId,
    FileName,
    FileUrl,
    FileType,
    UploadedByUserId,
    UploadedAt
)
VALUES
(
    @Id,
    @AppointmentId,
    @FileName,
    @FileUrl,
    @FileType,
    @UploadedByUserId,
    @UploadedAt
);";

        await connection.ExecuteAsync(
            sql,
            attachment);
    }

    public async Task<IEnumerable<AppointmentNote>>
        GetNotesAsync(
            Guid hospitalId,
            Guid appointmentId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    n.Id,
    n.AppointmentId,
    n.Note,
    n.CreatedByUserId,
    n.CreatedAt
FROM AppointmentNotes n
INNER JOIN Appointments a
    ON a.Id = n.AppointmentId
WHERE n.AppointmentId = @AppointmentId
AND a.HospitalId = @HospitalId
ORDER BY n.CreatedAt DESC;";

        return await connection.QueryAsync<AppointmentNote>(
            sql,
            new
            {
                AppointmentId = appointmentId,
                HospitalId = hospitalId
            });
    }

    public async Task<IEnumerable<AppointmentAttachment>>
        GetAttachmentsAsync(
            Guid hospitalId,
            Guid appointmentId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    aa.Id,
    aa.AppointmentId,
    aa.FileName,
    aa.FileUrl,
    aa.FileType,
    aa.UploadedByUserId,
    aa.UploadedAt
FROM AppointmentAttachments aa
INNER JOIN Appointments a
    ON a.Id = aa.AppointmentId
WHERE aa.AppointmentId = @AppointmentId
AND a.HospitalId = @HospitalId
ORDER BY aa.UploadedAt DESC;";

        return await connection.QueryAsync<AppointmentAttachment>(
            sql,
            new
            {
                AppointmentId = appointmentId,
                HospitalId = hospitalId
            });
    }

    private static AppointmentResponse MapResponse(
        AppointmentDbModel row)
    {
        return new AppointmentResponse
        {
            Id = row.Id,
            AppointmentNumber = row.AppointmentNumber,
            HospitalId = row.HospitalId,
            BranchId = row.BranchId,
            PatientId = row.PatientId,
            DoctorId = row.DoctorId,
            DepartmentId = row.DepartmentId,
            AppointmentStatusId = row.AppointmentStatusId,
            AppointmentStatusName = row.AppointmentStatusName,
            AppointmentDate = DateOnly.FromDateTime(row.AppointmentDate),
            AppointmentTime = row.AppointmentTime,
            TokenNumber = row.TokenNumber,
            ConsultationType = row.ConsultationType,
            ReasonForVisit = row.ReasonForVisit,
            Notes = row.Notes,
            IsFirstVisit = row.IsFirstVisit,
            CreatedAt = row.CreatedAt,
            UpdatedAt = row.UpdatedAt
        };
    }

    private class AppointmentDbModel
    {
        public Guid Id { get; set; }

        public string AppointmentNumber { get; set; } = string.Empty;

        public Guid HospitalId { get; set; }

        public Guid BranchId { get; set; }

        public Guid PatientId { get; set; }

        public Guid DoctorId { get; set; }

        public Guid DepartmentId { get; set; }

        public Guid AppointmentStatusId { get; set; }

        public string? AppointmentStatusName { get; set; }

        public DateTime AppointmentDate { get; set; }

        public TimeSpan AppointmentTime { get; set; }

        public int? TokenNumber { get; set; }

        public string ConsultationType { get; set; } = string.Empty;

        public string? ReasonForVisit { get; set; }

        public string? Notes { get; set; }

        public bool IsFirstVisit { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }

    private class CheckInAppointmentDbModel
    {
        public Guid Id { get; set; }

        public Guid DoctorId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string AppointmentStatusCode { get; set; } = string.Empty;
    }

    private class DoctorAvailableSlotDbModel
    {
        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
    }
}