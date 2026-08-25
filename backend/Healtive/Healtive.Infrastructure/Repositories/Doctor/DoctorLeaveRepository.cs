using Dapper;
using Healtive.Application.DTOs.DoctorLeave;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.Doctors;

public class DoctorLeaveRepository : IDoctorLeaveRepository
{
    private readonly IDbConnectionFactory _db;

    public DoctorLeaveRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    // =========================================================
    // DB MODEL
    // =========================================================

    private class DoctorLeaveDbModel
    {
        public Guid Id { get; set; }

        public Guid DoctorId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string? Reason { get; set; }

        public bool IsApproved { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    // =========================================================
    // GET DOCTOR
    // =========================================================

    public async Task<Doctor?> GetDoctorAsync(
        Guid hospitalId,
        Guid doctorId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    Id,
    HospitalId,
    UserId,
    FullName,
    DoctorCode,
    RegistrationNumber,
    Qualification,
    ExperienceYears,
    ConsultationFee,
    Gender,
    DateOfBirth,
    JoiningDate,
    Bio,
    ProfileImageUrl,
    IsAvailable,
    IsActive,
    CreatedAt,
    UpdatedAt,
    IsDeleted
FROM Doctors
WHERE Id = @DoctorId
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

        return await connection.QueryFirstOrDefaultAsync<Doctor>(
            sql,
            new
            {
                DoctorId = doctorId,
                HospitalId = hospitalId
            });
    }

    // =========================================================
    // GET BY ID
    // =========================================================

    public async Task<DoctorLeave?> GetByIdAsync(
        Guid doctorId,
        Guid id)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    Id,
    DoctorId,
    FromDate,
    ToDate,
    Reason,
    IsApproved,
    CreatedAt
FROM DoctorLeaves
WHERE Id = @Id
AND DoctorId = @DoctorId;";

        var row =
            await connection.QueryFirstOrDefaultAsync<DoctorLeaveDbModel>(
                sql,
                new
                {
                    Id = id,
                    DoctorId = doctorId
                });

        if (row == null)
            return null;

        return new DoctorLeave
        {
            Id = row.Id,
            DoctorId = row.DoctorId,
            FromDate = DateOnly.FromDateTime(row.FromDate),
            ToDate = DateOnly.FromDateTime(row.ToDate),
            Reason = row.Reason,
            IsApproved = row.IsApproved,
            CreatedAt = row.CreatedAt
        };
    }

    // =========================================================
    // CHECK OVERLAP
    // =========================================================

    public async Task<bool> HasOverlapAsync(
        Guid doctorId,
        DateOnly fromDate,
        DateOnly toDate,
        Guid? excludeId = null)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM DoctorLeaves
WHERE DoctorId = @DoctorId
AND
(
    FromDate <= @ToDate
    AND ToDate >= @FromDate
)
AND
(
    @ExcludeId IS NULL
    OR Id <> @ExcludeId
);";

        var count =
            await connection.ExecuteScalarAsync<int>(
                sql,
                new
                {
                    DoctorId = doctorId,

                    FromDate =
                        fromDate.ToDateTime(TimeOnly.MinValue),

                    ToDate =
                        toDate.ToDateTime(TimeOnly.MinValue),

                    ExcludeId = excludeId
                });

        return count > 0;
    }

    // =========================================================
    // CREATE
    // =========================================================

    public async Task CreateAsync(
        DoctorLeave leave)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
INSERT INTO DoctorLeaves
(
    Id,
    DoctorId,
    FromDate,
    ToDate,
    Reason,
    IsApproved,
    CreatedAt
)
VALUES
(
    @Id,
    @DoctorId,
    @FromDate,
    @ToDate,
    @Reason,
    @IsApproved,
    @CreatedAt
);";

        await connection.ExecuteAsync(
            sql,
            new
            {
                leave.Id,
                leave.DoctorId,

                FromDate =
                    leave.FromDate.ToDateTime(TimeOnly.MinValue),

                ToDate =
                    leave.ToDate.ToDateTime(TimeOnly.MinValue),

                leave.Reason,
                leave.IsApproved,
                leave.CreatedAt
            });
    }

    // =========================================================
    // UPDATE
    // =========================================================

    public async Task UpdateAsync(
        DoctorLeave leave)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE DoctorLeaves
SET
    FromDate = @FromDate,
    ToDate = @ToDate,
    Reason = @Reason,
    IsApproved = @IsApproved
WHERE Id = @Id
AND DoctorId = @DoctorId;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                leave.Id,
                leave.DoctorId,

                FromDate =
                    leave.FromDate.ToDateTime(TimeOnly.MinValue),

                ToDate =
                    leave.ToDate.ToDateTime(TimeOnly.MinValue),

                leave.Reason,
                leave.IsApproved
            });
    }

    // =========================================================
    // GET ALL LEAVES OF DOCTOR
    // =========================================================

    public async Task<IEnumerable<DoctorLeaveResponse>>
        GetByDoctorAsync(
            Guid hospitalId,
            Guid doctorId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    dl.Id,
    dl.DoctorId,
    dl.FromDate,
    dl.ToDate,
    dl.Reason,
    dl.IsApproved,
    dl.CreatedAt
FROM DoctorLeaves dl
INNER JOIN Doctors d
    ON d.Id = dl.DoctorId
WHERE dl.DoctorId = @DoctorId
AND d.HospitalId = @HospitalId
AND d.IsDeleted = 0
ORDER BY dl.FromDate DESC;";

        var rows =
            await connection.QueryAsync<DoctorLeaveDbModel>(
                sql,
                new
                {
                    DoctorId = doctorId,
                    HospitalId = hospitalId
                });

        return rows.Select(x => new DoctorLeaveResponse
        {
            Id = x.Id,
            DoctorId = x.DoctorId,

            FromDate =
                DateOnly.FromDateTime(x.FromDate),

            ToDate =
                DateOnly.FromDateTime(x.ToDate),

            Reason = x.Reason,
            IsApproved = x.IsApproved,
            CreatedAt = x.CreatedAt
        });
    }

    // =========================================================
    // DELETE
    // =========================================================

    public async Task DeleteAsync(
        Guid doctorId,
        Guid id)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
DELETE FROM DoctorLeaves
WHERE Id = @Id
AND DoctorId = @DoctorId;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                Id = id,
                DoctorId = doctorId
            });
    }

    // =========================================================
    // APPROVE
    // =========================================================

    public async Task ApproveAsync(
        Guid doctorId,
        Guid id)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE DoctorLeaves
SET IsApproved = 1
WHERE Id = @Id
AND DoctorId = @DoctorId;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                Id = id,
                DoctorId = doctorId
            });
    }

    // =========================================================
    // REJECT
    // =========================================================

    public async Task RejectAsync(
        Guid doctorId,
        Guid id)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE DoctorLeaves
SET IsApproved = 0
WHERE Id = @Id
AND DoctorId = @DoctorId;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                Id = id,
                DoctorId = doctorId
            });
    }
}