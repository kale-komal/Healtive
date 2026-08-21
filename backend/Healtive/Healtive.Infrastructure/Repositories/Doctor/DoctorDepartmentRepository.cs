using Dapper;
using Healtive.Application.DTOs.Doctor;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.Doctors;

public class DoctorDepartmentRepository : IDoctorDepartmentRepository
{
    private readonly IDbConnectionFactory _db;

    public DoctorDepartmentRepository(IDbConnectionFactory db)
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

    public async Task<Department?> GetDepartmentAsync(
        Guid hospitalId,
        Guid departmentId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    Id,
    HospitalId,
    Name,
    Code,
    Description,
    IsActive,
    CreatedAt,
    UpdatedAt,
    IsDeleted
FROM Departments
WHERE Id = @DepartmentId
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

        return await connection.QueryFirstOrDefaultAsync<Department>(
            sql,
            new
            {
                DepartmentId = departmentId,
                HospitalId = hospitalId
            });
    }

    public async Task<bool> MappingExistsAsync(
        Guid doctorId,
        Guid departmentId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM DoctorDepartment
WHERE DoctorId = @DoctorId
AND DepartmentId = @DepartmentId;";

        var count = await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                DoctorId = doctorId,
                DepartmentId = departmentId
            });

        return count > 0;
    }

    public async Task AssignAsync(
        DoctorDepartment mapping)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
INSERT INTO DoctorDepartment
(
    DoctorId,
    DepartmentId,
    CreatedAt
)
VALUES
(
    @DoctorId,
    @DepartmentId,
    @CreatedAt
);";

        await connection.ExecuteAsync(
            sql,
            mapping);
    }

    public async Task<IEnumerable<DoctorDepartmentResponse>>
        GetDoctorDepartmentsAsync(
            Guid hospitalId,
            Guid doctorId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    dd.DoctorId,
    dd.DepartmentId,
    d.Name AS DepartmentName,
    dd.CreatedAt
FROM DoctorDepartment dd
INNER JOIN Departments d
    ON d.Id = dd.DepartmentId
WHERE dd.DoctorId = @DoctorId
AND d.HospitalId = @HospitalId
AND d.IsDeleted = 0
ORDER BY d.Name;";

        return await connection.QueryAsync<DoctorDepartmentResponse>(
            sql,
            new
            {
                DoctorId = doctorId,
                HospitalId = hospitalId
            });
    }

    public async Task RemoveAsync(
        Guid doctorId,
        Guid departmentId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
DELETE FROM DoctorDepartment
WHERE DoctorId = @DoctorId
AND DepartmentId = @DepartmentId;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                DoctorId = doctorId,
                DepartmentId = departmentId
            });
    }

    public async Task<IEnumerable<DoctorListResponse>>
        GetDepartmentDoctorsAsync(
            Guid hospitalId,
            Guid departmentId)
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
FROM DoctorDepartment dd
INNER JOIN Doctors d
    ON d.Id = dd.DoctorId
WHERE dd.DepartmentId = @DepartmentId
AND d.HospitalId = @HospitalId
AND d.IsDeleted = 0
ORDER BY d.FullName;";

        return await connection.QueryAsync<DoctorListResponse>(
            sql,
            new
            {
                DepartmentId = departmentId,
                HospitalId = hospitalId
            });
    }
}