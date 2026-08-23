using Dapper;
using Healtive.Application.DTOs.DoctorAvailability;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.Doctors;

public class DoctorAvailabilityRepository
    : IDoctorAvailabilityRepository
{
    private readonly IDbConnectionFactory _db;

    public DoctorAvailabilityRepository(
        IDbConnectionFactory db)
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

    public async Task<DoctorAvailability?> GetByIdAsync(
        Guid doctorId,
        Guid id)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    Id,
    DoctorId,
    DayOfWeek,
    StartTime,
    EndTime,
    MaxAppointments,
    IsAvailable,
    CreatedAt
FROM doctoravailability
WHERE Id = @Id
AND DoctorId = @DoctorId;";

        return await connection.QueryFirstOrDefaultAsync<DoctorAvailability>(
            sql,
            new
            {
                Id = id,
                DoctorId = doctorId
            });
    }

    public async Task<bool> HasOverlapAsync(
        Guid doctorId,
        byte dayOfWeek,
        TimeSpan startTime,
        TimeSpan endTime,
        Guid? excludeId = null)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM doctoravailability
WHERE DoctorId = @DoctorId
AND DayOfWeek = @DayOfWeek
AND IsAvailable = 1
AND
(
    StartTime < @EndTime
    AND EndTime > @StartTime
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
                DayOfWeek = dayOfWeek,
                StartTime = startTime,
                EndTime = endTime,
                ExcludeId = excludeId
            });

        return count > 0;
    }

    public async Task CreateAsync(
        DoctorAvailability availability)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
INSERT INTO doctoravailability
(
    Id,
    DoctorId,
    DayOfWeek,
    StartTime,
    EndTime,
    MaxAppointments,
    IsAvailable,
    CreatedAt
)
VALUES
(
    @Id,
    @DoctorId,
    @DayOfWeek,
    @StartTime,
    @EndTime,
    @MaxAppointments,
    @IsAvailable,
    @CreatedAt
);";

        await connection.ExecuteAsync(
            sql,
            availability);
    }

    public async Task UpdateAsync(
        DoctorAvailability availability)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE doctoravailability
SET
    DayOfWeek = @DayOfWeek,
    StartTime = @StartTime,
    EndTime = @EndTime,
    MaxAppointments = @MaxAppointments,
    IsAvailable = @IsAvailable
WHERE Id = @Id
AND DoctorId = @DoctorId;";

        await connection.ExecuteAsync(
            sql,
            availability);
    }

    public async Task<IEnumerable<DoctorAvailabilityResponse>>
        GetByDoctorAsync(
            Guid hospitalId,
            Guid doctorId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    da.Id,
    da.DoctorId,
    da.DayOfWeek,
    da.StartTime,
    da.EndTime,
    da.MaxAppointments,
    da.IsAvailable,
    da.CreatedAt
FROM doctoravailability da
INNER JOIN Doctors d
    ON d.Id = da.DoctorId
WHERE da.DoctorId = @DoctorId
AND d.HospitalId = @HospitalId
AND d.IsDeleted = 0
ORDER BY da.DayOfWeek, da.StartTime;";

        return await connection.QueryAsync<DoctorAvailabilityResponse>(
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
DELETE FROM doctoravailability
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

    public async Task ActivateAsync(
        Guid doctorId,
        Guid id)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE doctoravailability
SET IsAvailable = 1
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

    public async Task DeactivateAsync(
        Guid doctorId,
        Guid id)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE doctoravailability
SET IsAvailable = 0
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