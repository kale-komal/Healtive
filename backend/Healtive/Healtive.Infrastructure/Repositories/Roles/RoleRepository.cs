using Dapper;
using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Role;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.Roles;

public class RoleRepository : IRoleRepository
{
    private readonly IDbConnectionFactory _db;

    public RoleRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<bool> ExistsByNameAsync(
        Guid hospitalId,
        string name)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Roles
WHERE HospitalId = @HospitalId
AND LOWER(Name) = LOWER(@Name)
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                HospitalId = hospitalId,
                Name = name.Trim()
            }) > 0;
    }

    public async Task<bool> ExistsByNameAsync(
        Guid hospitalId,
        Guid roleId,
        string name)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Roles
WHERE HospitalId = @HospitalId
AND LOWER(Name) = LOWER(@Name)
AND Id <> @RoleId
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                HospitalId = hospitalId,
                RoleId = roleId,
                Name = name.Trim()
            }) > 0;
    }

    public async Task CreateAsync(Role role)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
INSERT INTO Roles
(
    Id,
    HospitalId,
    Name,
    Description,
    IsSystemRole,
    IsActive,
    CreatedAt,
    IsDeleted
)
VALUES
(
    @Id,
    @HospitalId,
    @Name,
    @Description,
    @IsSystemRole,
    @IsActive,
    @CreatedAt,
    @IsDeleted
);";

        await connection.ExecuteAsync(sql, role);
    }

    public async Task UpdateAsync(Role role)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Roles
SET
    Name = @Name,
    Description = @Description,
    UpdatedAt = @UpdatedAt
WHERE Id = @Id
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

        await connection.ExecuteAsync(sql, role);
    }

    public async Task<PagedResponse<RoleListResponse>> GetAllAsync(
        Guid hospitalId,
        RoleFilterRequest request)
    {
        using var connection = _db.CreateConnection();

        var conditions = @"
WHERE HospitalId = @HospitalId
AND IsDeleted = 0";

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            conditions += @"
AND (
    Name LIKE @Search
    OR Description LIKE @Search
)";
        }

        if (request.Status.HasValue)
        {
            conditions += @"
AND IsActive = @Status";
        }

        const string countBase = @"
SELECT COUNT(*)
FROM Roles";

        var countSql = countBase + conditions;

        var totalCount = await connection.ExecuteScalarAsync<int>(
            countSql,
            new
            {
                HospitalId = hospitalId,
                Search = $"%{request.Search}%",
                request.Status
            });

        var offset = (request.Page - 1) * request.PageSize;

        var sql = $@"
SELECT
    Id AS RoleId,
    Name,
    Description,
    IsSystemRole,
    IsActive,
    CreatedAt
FROM Roles
{conditions}
ORDER BY IsSystemRole DESC, CreatedAt ASC
LIMIT @PageSize OFFSET @Offset;";

        var items = await connection.QueryAsync<RoleListResponse>(
            sql,
            new
            {
                HospitalId = hospitalId,
                Search = $"%{request.Search}%",
                request.Status,
                request.PageSize,
                Offset = offset
            });

        var totalPages = request.PageSize == 0
            ? 0
            : (int)Math.Ceiling(
                totalCount / (double)request.PageSize);

        return new PagedResponse<RoleListResponse>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }

    public async Task<RoleResponse?> GetByIdAsync(
        Guid hospitalId,
        Guid roleId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    Id AS RoleId,
    HospitalId,
    Name,
    Description,
    IsSystemRole,
    IsActive,
    CreatedAt,
    UpdatedAt
FROM Roles
WHERE Id = @RoleId
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

        return await connection.QueryFirstOrDefaultAsync<RoleResponse>(
            sql,
            new
            {
                HospitalId = hospitalId,
                RoleId = roleId
            });
    }

    public async Task<Role?> GetEntityByIdAsync(
        Guid hospitalId,
        Guid roleId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT *
FROM Roles
WHERE Id = @RoleId
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

        return await connection.QueryFirstOrDefaultAsync<Role>(
            sql,
            new
            {
                HospitalId = hospitalId,
                RoleId = roleId
            });
    }

    public async Task DeleteAsync(
        Guid hospitalId,
        Guid roleId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Roles
SET
    IsDeleted = 1,
    IsActive = 0,
    UpdatedAt = @UpdatedAt
WHERE Id = @RoleId
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                HospitalId = hospitalId,
                RoleId = roleId,
                UpdatedAt = DateTime.UtcNow
            });
    }

    public async Task ActivateAsync(
        Guid hospitalId,
        Guid roleId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Roles
SET
    IsActive = 1,
    UpdatedAt = @UpdatedAt
WHERE Id = @RoleId
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                HospitalId = hospitalId,
                RoleId = roleId,
                UpdatedAt = DateTime.UtcNow
            });
    }

    public async Task DeactivateAsync(
        Guid hospitalId,
        Guid roleId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Roles
SET
    IsActive = 0,
    UpdatedAt = @UpdatedAt
WHERE Id = @RoleId
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                HospitalId = hospitalId,
                RoleId = roleId,
                UpdatedAt = DateTime.UtcNow
            });
    }
}