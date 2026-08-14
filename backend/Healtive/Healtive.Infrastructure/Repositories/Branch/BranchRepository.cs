using Dapper;
using Healtive.Application.DTOs.Branch;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;
using Healtive.Infrastructure.Data;
using Healtive.Application.DTOs.Common;
namespace Healtive.Infrastructure.Repositories.Branches;

public class BranchRepository : IBranchRepository
{
    private readonly IDbConnectionFactory _db;

    public BranchRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task CreateAsync(Branch branch)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
INSERT INTO Branches
(
    Id,
    HospitalId,
    Name,
    Code,
    Email,
    PhoneNumber,
    Address,
    City,
    State,
    Country,
    PostalCode,
    IsHeadOffice,
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
    @Email,
    @PhoneNumber,
    @Address,
    @City,
    @State,
    @Country,
    @PostalCode,
    @IsHeadOffice,
    @IsActive,
    @CreatedAt,
    @IsDeleted
);";

        await connection.ExecuteAsync(sql, branch);
    }

    public async Task<PagedResponse<BranchListResponse>> GetAllAsync(
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
FROM Branches
WHERE HospitalId = @HospitalId
AND IsDeleted = 0
AND (
    @Search IS NULL
    OR @Search = ''
    OR Name LIKE CONCAT('%', @Search, '%')
    OR Code LIKE CONCAT('%', @Search, '%')
    OR City LIKE CONCAT('%', @Search, '%')
);";

        var totalCount = await connection.ExecuteScalarAsync<int>(
            countSql,
            new
            {
                HospitalId = hospitalId,
                Search = search
            });

        const string sql = @"
SELECT
    Id AS BranchId,
    Name,
    Code,
    PhoneNumber,
    City,
    State,
    IsHeadOffice,
    IsActive,
    CreatedAt
FROM Branches
WHERE HospitalId = @HospitalId
AND IsDeleted = 0
AND (
    @Search IS NULL
    OR @Search = ''
    OR Name LIKE CONCAT('%', @Search, '%')
    OR Code LIKE CONCAT('%', @Search, '%')
    OR City LIKE CONCAT('%', @Search, '%')
)
AND (
    @Status IS NULL
    OR IsActive = @Status
)
ORDER BY CreatedAt DESC
LIMIT @PageSize OFFSET @Offset;";

        var items = await connection.QueryAsync<BranchListResponse>(
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

        return new PagedResponse<BranchListResponse>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }

    public async Task<Branch?> GetByIdAsync(
        Guid hospitalId,
        Guid branchId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    Id,
    HospitalId,
    Name,
    Code,
    Email,
    PhoneNumber,
    Address,
    City,
    State,
    Country,
    PostalCode,
    IsHeadOffice,
    IsActive,
    CreatedAt,
    UpdatedAt,
    IsDeleted
FROM Branches
WHERE Id = @BranchId
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

        return await connection.QueryFirstOrDefaultAsync<Branch>(
            sql,
            new
            {
                BranchId = branchId,
                HospitalId = hospitalId
            });
    }

    public async Task UpdateAsync(Branch branch)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Branches
SET
    Name = @Name,
    Code = @Code,
    Email = @Email,
    PhoneNumber = @PhoneNumber,
    Address = @Address,
    City = @City,
    State = @State,
    Country = @Country,
    PostalCode = @PostalCode,
    IsHeadOffice = @IsHeadOffice,
    UpdatedAt = @UpdatedAt
WHERE Id = @Id
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

        await connection.ExecuteAsync(sql, branch);
    }

    public async Task DeleteAsync(
        Guid hospitalId,
        Guid branchId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Branches
SET
    IsDeleted = 1,
    IsActive = 0,
    UpdatedAt = @UpdatedAt
WHERE Id = @BranchId
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                BranchId = branchId,
                HospitalId = hospitalId,
                UpdatedAt = DateTime.UtcNow
            });
    }

    public async Task<bool> ExistsByCodeAsync(
        Guid hospitalId,
        string code)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Branches
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
        Guid branchId,
        string code)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Branches
WHERE HospitalId = @HospitalId
AND Code = @Code
AND Id <> @BranchId
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                HospitalId = hospitalId,
                BranchId = branchId,
                Code = code
            }) > 0;
    }

    public async Task ActivateAsync(
        Guid hospitalId,
        Guid branchId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Branches
SET
    IsActive = 1,
    UpdatedAt = @UpdatedAt
WHERE Id = @BranchId
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                BranchId = branchId,
                HospitalId = hospitalId,
                UpdatedAt = DateTime.UtcNow
            });
    }

    public async Task DeactivateAsync(
        Guid hospitalId,
        Guid branchId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Branches
SET
    IsActive = 0,
    UpdatedAt = @UpdatedAt
WHERE Id = @BranchId
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                BranchId = branchId,
                HospitalId = hospitalId,
                UpdatedAt = DateTime.UtcNow
            });
    }

    public async Task ClearHeadOfficeAsync(Guid hospitalId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Branches
SET
    IsHeadOffice = 0,
    UpdatedAt = @UpdatedAt
WHERE HospitalId = @HospitalId
AND IsDeleted = 0;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                HospitalId = hospitalId,
                UpdatedAt = DateTime.UtcNow
            });
    }
}