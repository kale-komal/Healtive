using Dapper;
using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Department;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.Departments;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly IDbConnectionFactory _db;

    public DepartmentRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task CreateAsync(Department department)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
INSERT INTO Departments
(
    Id,
    HospitalId,
    Name,
    Code,
    Description,
    IsActive,
    CreatedAt,
    IsDeleted
)
VALUES
(
    @Id,
    @HospitalId,
    @Name,
    @Code,
    @Description,
    @IsActive,
    @CreatedAt,
    @IsDeleted
);";

        await connection.ExecuteAsync(sql, department);
    }

    public async Task<PagedResponse<DepartmentListResponse>> GetAllAsync(
        Guid hospitalId,
        string? search,
        bool? status,
        int page,
        int pageSize)
    {
        using var connection = _db.CreateConnection();

        var offset = (page - 1) * pageSize;

        const string countSql = @"
SELECT COUNT(*)
FROM Departments
WHERE HospitalId = @HospitalId
AND IsDeleted = 0
AND
(
    @Search IS NULL
    OR @Search = ''
    OR Name LIKE CONCAT('%', @Search, '%')
    OR Code LIKE CONCAT('%', @Search, '%')
)
AND
(
    @Status IS NULL
    OR IsActive = @Status
);";

        var totalCount = await connection.ExecuteScalarAsync<int>(
            countSql,
            new
            {
                HospitalId = hospitalId,
                Search = search,
                Status = status
            });

        const string sql = @"
SELECT
    Id AS DepartmentId,
    Name,
    Code,
    Description,
    IsActive,
    CreatedAt
FROM Departments
WHERE HospitalId = @HospitalId
AND IsDeleted = 0
AND
(
    @Search IS NULL
    OR @Search = ''
    OR Name LIKE CONCAT('%', @Search, '%')
    OR Code LIKE CONCAT('%', @Search, '%')
)
AND
(
    @Status IS NULL
    OR IsActive = @Status
)
ORDER BY CreatedAt DESC
LIMIT @PageSize OFFSET @Offset;";

        var items = await connection.QueryAsync<DepartmentListResponse>(
            sql,
            new
            {
                HospitalId = hospitalId,
                Search = search,
                Status = status,
                PageSize = pageSize,
                Offset = offset
            });

        var totalPages = (int)Math.Ceiling(
            totalCount / (double)pageSize);

        return new PagedResponse<DepartmentListResponse>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }

    public async Task<Department?> GetByIdAsync(
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
                HospitalId = hospitalId,
                DepartmentId = departmentId
            });
    }

    public async Task<bool> ExistsByCodeAsync(
        Guid hospitalId,
        string code)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Departments
WHERE HospitalId = @HospitalId
AND Code = @Code
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                HospitalId = hospitalId,
                Code = code
            }) > 0;
    }

    public async Task<bool> ExistsByCodeAsync(
        Guid hospitalId,
        Guid departmentId,
        string code)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Departments
WHERE HospitalId = @HospitalId
AND Code = @Code
AND Id <> @DepartmentId
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                HospitalId = hospitalId,
                DepartmentId = departmentId,
                Code = code
            }) > 0;
    }

    public async Task UpdateAsync(Department department)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Departments
SET
    Name = @Name,
    Code = @Code,
    Description = @Description,
    UpdatedAt = @UpdatedAt
WHERE Id = @Id
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

        await connection.ExecuteAsync(sql, department);
    }

    public async Task DeleteAsync(
        Guid hospitalId,
        Guid departmentId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Departments
SET
    IsDeleted = 1,
    IsActive = 0,
    UpdatedAt = @UpdatedAt
WHERE Id = @DepartmentId
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                HospitalId = hospitalId,
                DepartmentId = departmentId,
                UpdatedAt = DateTime.UtcNow
            });
    }

    public async Task ActivateAsync(
        Guid hospitalId,
        Guid departmentId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Departments
SET
    IsActive = 1,
    UpdatedAt = @UpdatedAt
WHERE Id = @DepartmentId
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                HospitalId = hospitalId,
                DepartmentId = departmentId,
                UpdatedAt = DateTime.UtcNow
            });
    }

    public async Task DeactivateAsync(
        Guid hospitalId,
        Guid departmentId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Departments
SET
    IsActive = 0,
    UpdatedAt = @UpdatedAt
WHERE Id = @DepartmentId
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                HospitalId = hospitalId,
                DepartmentId = departmentId,
                UpdatedAt = DateTime.UtcNow
            });
    }
}