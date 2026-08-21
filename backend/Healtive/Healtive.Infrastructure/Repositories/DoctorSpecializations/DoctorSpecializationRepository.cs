using Dapper;
using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.DoctorSpecialization;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.DoctorSpecializations;

public class DoctorSpecializationRepository
    : IDoctorSpecializationRepository
{
    private readonly IDbConnectionFactory _db;

    public DoctorSpecializationRepository(
        IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<bool> ExistsByNameAsync(
        string name)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM DoctorSpecializations
WHERE LOWER(Name) = LOWER(@Name)
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                Name = name.Trim()
            }) > 0;
    }

    public async Task<bool> ExistsByNameAsync(
        Guid id,
        string name)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM DoctorSpecializations
WHERE LOWER(Name) = LOWER(@Name)
AND Id <> @Id
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                Id = id,
                Name = name.Trim()
            }) > 0;
    }

    public async Task<bool> ExistsByCodeAsync(
        string code)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM DoctorSpecializations
WHERE LOWER(Code) = LOWER(@Code)
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                Code = code.Trim()
            }) > 0;
    }

    public async Task<bool> ExistsByCodeAsync(
        Guid id,
        string code)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM DoctorSpecializations
WHERE LOWER(Code) = LOWER(@Code)
AND Id <> @Id
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                Id = id,
                Code = code.Trim()
            }) > 0;
    }

    public async Task CreateAsync(
        DoctorSpecialization specialization)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
INSERT INTO DoctorSpecializations
(
    Id,
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
    @Name,
    @Code,
    @Description,
    @IsActive,
    @CreatedAt,
    @IsDeleted
);";

        await connection.ExecuteAsync(
            sql,
            specialization);
    }

    public async Task UpdateAsync(
        DoctorSpecialization specialization)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE DoctorSpecializations
SET
    Name = @Name,
    Code = @Code,
    Description = @Description,
    UpdatedAt = @UpdatedAt
WHERE Id = @Id
AND IsDeleted = 0;";

        await connection.ExecuteAsync(
            sql,
            specialization);
    }

    public async Task<PagedResponse<DoctorSpecializationResponse>>
        GetAllAsync(
            DoctorSpecializationFilterRequest request)
    {
        using var connection = _db.CreateConnection();

        var conditions = @"
WHERE IsDeleted = 0";

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            conditions += @"
AND
(
    Name LIKE @Search
    OR Code LIKE @Search
    OR Description LIKE @Search
)";
        }

        if (request.Status.HasValue)
        {
            conditions += @"
AND IsActive = @Status";
        }

        var countSql = $@"
SELECT COUNT(*)
FROM DoctorSpecializations
{conditions};";

        var totalCount =
            await connection.ExecuteScalarAsync<int>(
                countSql,
                new
                {
                    Search = $"%{request.Search}%",
                    request.Status
                });

        var offset =
            (request.Page - 1) * request.PageSize;

        var sql = $@"
SELECT
    Id AS SpecializationId,
    Name,
    Code,
    Description,
    IsActive,
    CreatedAt,
    UpdatedAt
FROM DoctorSpecializations
{conditions}
ORDER BY Name ASC
LIMIT @PageSize OFFSET @Offset;";

        var items =
            await connection.QueryAsync<
                DoctorSpecializationResponse>(
                sql,
                new
                {
                    Search = $"%{request.Search}%",
                    request.Status,
                    request.PageSize,
                    Offset = offset
                });

        var totalPages =
            request.PageSize == 0
                ? 0
                : (int)Math.Ceiling(
                    totalCount /
                    (double)request.PageSize);

        return new PagedResponse<
            DoctorSpecializationResponse>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }

    public async Task<DoctorSpecializationResponse?>
        GetByIdAsync(Guid id)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    Id AS SpecializationId,
    Name,
    Code,
    Description,
    IsActive,
    CreatedAt,
    UpdatedAt
FROM DoctorSpecializations
WHERE Id = @Id
AND IsDeleted = 0;";

        return await connection.QueryFirstOrDefaultAsync<
            DoctorSpecializationResponse>(
            sql,
            new
            {
                Id = id
            });
    }

    public async Task<DoctorSpecialization?>
        GetEntityByIdAsync(Guid id)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT *
FROM DoctorSpecializations
WHERE Id = @Id
AND IsDeleted = 0;";

        return await connection.QueryFirstOrDefaultAsync<
            DoctorSpecialization>(
            sql,
            new
            {
                Id = id
            });
    }

    public async Task DeleteAsync(Guid id)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE DoctorSpecializations
SET
    IsDeleted = 1,
    IsActive = 0,
    UpdatedAt = @UpdatedAt
WHERE Id = @Id
AND IsDeleted = 0;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                Id = id,
                UpdatedAt = DateTime.UtcNow
            });
    }

    public async Task ActivateAsync(Guid id)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE DoctorSpecializations
SET
    IsActive = 1,
    UpdatedAt = @UpdatedAt
WHERE Id = @Id
AND IsDeleted = 0;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                Id = id,
                UpdatedAt = DateTime.UtcNow
            });
    }

    public async Task DeactivateAsync(Guid id)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE DoctorSpecializations
SET
    IsActive = 0,
    UpdatedAt = @UpdatedAt
WHERE Id = @Id
AND IsDeleted = 0;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                Id = id,
                UpdatedAt = DateTime.UtcNow
            });
    }
}