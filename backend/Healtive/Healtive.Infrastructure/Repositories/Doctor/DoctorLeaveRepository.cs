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

        return await connection.QueryFirstOrDefaultAsync<DoctorLeave>(
            sql,
            new
            {
                Id = id,
                DoctorId = doctorId
            });
    }

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

        var count = await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                DoctorId = doctorId,
                FromDate = fromDate,
                ToDate = toDate,
                ExcludeId = excludeId
            });

        return count > 0;
    }

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
            leave);
    }

    public async Task UpdateAsync(
        DoctorLeave leave)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE DoctorLeaves
SET
    FromDate = @FromDate,
    ToDate = @ToDate,
    Reason = @Reason
WHERE Id = @Id
AND DoctorId = @DoctorId;";

        await connection.ExecuteAsync(
            sql,
            leave);
    }

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

        return await connection.QueryAsync<DoctorLeaveResponse>(
            sql,
            new
            {
                DoctorId = doctorId,
                HospitalId = hospitalId
            });
    }

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