using Dapper;
using Healtive.Application.DTOs.Doctor;
using Healtive.Application.DTOs.DoctorSpecialization;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.DoctorSpecializations;

public class DoctorSpecializationMappingRepository
    : IDoctorSpecializationMappingRepository
{
    private readonly IDbConnectionFactory _db;

    public DoctorSpecializationMappingRepository(
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

    public async Task<DoctorSpecialization?>
        GetSpecializationAsync(
            Guid specializationId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    Id,
    Name,
    Code,
    Description,
    IsActive,
    CreatedAt,
    UpdatedAt,
    IsDeleted
FROM DoctorSpecializations
WHERE Id = @SpecializationId
AND IsDeleted = 0;";

        return await connection.QueryFirstOrDefaultAsync<
            DoctorSpecialization>(
            sql,
            new
            {
                SpecializationId = specializationId
            });
    }

    public async Task<bool> MappingExistsAsync(
        Guid doctorId,
        Guid specializationId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM DoctorSpecializationMappings
WHERE DoctorId = @DoctorId
AND SpecializationId = @SpecializationId;";

        var count = await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                DoctorId = doctorId,
                SpecializationId = specializationId
            });

        return count > 0;
    }

    public async Task AssignAsync(
        DoctorSpecializationMapping mapping)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
INSERT INTO DoctorSpecializationMappings
(
    DoctorId,
    SpecializationId,
    CreatedAt
)
VALUES
(
    @DoctorId,
    @SpecializationId,
    @CreatedAt
);";

        await connection.ExecuteAsync(
            sql,
            mapping);
    }

    public async Task<IEnumerable<DoctorSpecializationMappingResponse>>
        GetDoctorSpecializationsAsync(
            Guid hospitalId,
            Guid doctorId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    dsm.DoctorId,
    dsm.SpecializationId,
    ds.Name AS SpecializationName,
    ds.Code AS SpecializationCode,
    dsm.CreatedAt
FROM DoctorSpecializationMappings dsm
INNER JOIN DoctorSpecializations ds
    ON ds.Id = dsm.SpecializationId
INNER JOIN Doctors d
    ON d.Id = dsm.DoctorId
WHERE dsm.DoctorId = @DoctorId
AND d.HospitalId = @HospitalId
AND ds.IsDeleted = 0
ORDER BY ds.Name;";

        return await connection.QueryAsync<
            DoctorSpecializationMappingResponse>(
            sql,
            new
            {
                DoctorId = doctorId,
                HospitalId = hospitalId
            });
    }

    public async Task RemoveAsync(
        Guid doctorId,
        Guid specializationId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
DELETE FROM DoctorSpecializationMappings
WHERE DoctorId = @DoctorId
AND SpecializationId = @SpecializationId;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                DoctorId = doctorId,
                SpecializationId = specializationId
            });
    }

    public async Task<IEnumerable<DoctorListResponse>>
        GetSpecializationDoctorsAsync(
            Guid hospitalId,
            Guid specializationId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    d.Id AS DoctorId,
    d.FullName,
    d.DoctorCode,
    d.RegistrationNumber,
    d.Qualification,
    d.ExperienceYears,
    d.ConsultationFee,
    d.Gender,
    d.IsAvailable,
    d.IsActive,
    d.CreatedAt
FROM DoctorSpecializationMappings dsm
INNER JOIN Doctors d
    ON d.Id = dsm.DoctorId
INNER JOIN DoctorSpecializations ds
    ON ds.Id = dsm.SpecializationId
WHERE dsm.SpecializationId = @SpecializationId
AND d.HospitalId = @HospitalId
AND d.IsDeleted = 0
AND ds.IsDeleted = 0
ORDER BY d.FullName;";

        return await connection.QueryAsync<DoctorListResponse>(
            sql,
            new
            {
                HospitalId = hospitalId,
                SpecializationId = specializationId
            });
    }
}