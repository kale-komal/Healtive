using Dapper;
using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Staff;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.Staff;

public class StaffRepository : IStaffRepository
{
    private readonly IDbConnectionFactory _db;

    public StaffRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<bool> UsernameExistsAsync(
        Guid hospitalId,
        string username)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Users
WHERE HospitalId = @HospitalId
AND Username = @Username
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                HospitalId = hospitalId,
                Username = username
            }) > 0;
    }

    public async Task<bool> EmployeeCodeExistsAsync(
        Guid hospitalId,
        string employeeCode)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Users
WHERE HospitalId = @HospitalId
AND EmployeeCode = @EmployeeCode
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                HospitalId = hospitalId,
                EmployeeCode = employeeCode
            }) > 0;
    }

    public async Task<bool> EmailExistsAsync(
        Guid hospitalId,
        string email)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Users
WHERE HospitalId = @HospitalId
AND Email = @Email
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                HospitalId = hospitalId,
                Email = email
            }) > 0;
    }

    public async Task<bool> MobileNumberExistsAsync(
        Guid hospitalId,
        string mobileNumber)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Users
WHERE HospitalId = @HospitalId
AND MobileNumber = @MobileNumber
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                HospitalId = hospitalId,
                MobileNumber = mobileNumber
            }) > 0;
    }

    public async Task<bool> UsernameExistsAsync(
        Guid hospitalId,
        Guid userId,
        string username)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Users
WHERE HospitalId = @HospitalId
AND Username = @Username
AND Id <> @UserId
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                HospitalId = hospitalId,
                UserId = userId,
                Username = username
            }) > 0;
    }

    public async Task<bool> EmployeeCodeExistsAsync(
        Guid hospitalId,
        Guid userId,
        string employeeCode)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Users
WHERE HospitalId = @HospitalId
AND EmployeeCode = @EmployeeCode
AND Id <> @UserId
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                HospitalId = hospitalId,
                UserId = userId,
                EmployeeCode = employeeCode
            }) > 0;
    }

    public async Task<bool> EmailExistsAsync(
        Guid hospitalId,
        Guid userId,
        string email)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Users
WHERE HospitalId = @HospitalId
AND Email = @Email
AND Id <> @UserId
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                HospitalId = hospitalId,
                UserId = userId,
                Email = email
            }) > 0;
    }

    public async Task<bool> MobileNumberExistsAsync(
        Guid hospitalId,
        Guid userId,
        string mobileNumber)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Users
WHERE HospitalId = @HospitalId
AND MobileNumber = @MobileNumber
AND Id <> @UserId
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                HospitalId = hospitalId,
                UserId = userId,
                MobileNumber = mobileNumber
            }) > 0;
    }

    public async Task<bool> RoleExistsAsync(
        Guid hospitalId,
        Guid roleId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Roles
WHERE Id = @RoleId
AND HospitalId = @HospitalId
AND IsActive = 1
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                HospitalId = hospitalId,
                RoleId = roleId
            }) > 0;
    }

    public async Task CreateAsync(User user)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
INSERT INTO Users
(
    Id,
    HospitalId,
    EmployeeCode,
    Username,
    FirstName,
    LastName,
    Email,
    MobileNumber,
    PasswordHash,
    IsEmailVerified,
    IsMobileVerified,
    IsActive,
    CreatedAt,
    IsDeleted
)
VALUES
(
    @Id,
    @HospitalId,
    @EmployeeCode,
    @Username,
    @FirstName,
    @LastName,
    @Email,
    @MobileNumber,
    @PasswordHash,
    @IsEmailVerified,
    @IsMobileVerified,
    @IsActive,
    @CreatedAt,
    @IsDeleted
);";

        await connection.ExecuteAsync(sql, user);
    }

    public async Task AssignRoleAsync(UserRole userRole)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
INSERT INTO UserRoles
(
    UserId,
    RoleId,
    AssignedAt
)
VALUES
(
    @UserId,
    @RoleId,
    @AssignedAt
);";

        await connection.ExecuteAsync(sql, userRole);
    }

    public async Task UpdateAsync(User user)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Users
SET
    EmployeeCode = @EmployeeCode,
    FirstName = @FirstName,
    LastName = @LastName,
    Email = @Email,
    MobileNumber = @MobileNumber,
    UpdatedAt = @UpdatedAt
WHERE Id = @Id
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

        await connection.ExecuteAsync(sql, user);
    }

    public async Task UpdateRoleAsync(
        Guid userId,
        Guid roleId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE UserRoles
SET RoleId = @RoleId
WHERE UserId = @UserId;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                UserId = userId,
                RoleId = roleId
            });
    }

    public async Task<PagedResponse<StaffListResponse>> GetAllAsync(
        StaffFilterRequest request,
        Guid hospitalId)
    {
        using var connection = _db.CreateConnection();

        var conditions = @"
WHERE u.HospitalId = @HospitalId
AND u.IsDeleted = 0";

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            conditions += @"
AND (
    u.EmployeeCode LIKE @Search
    OR u.Username LIKE @Search
    OR u.FirstName LIKE @Search
    OR u.LastName LIKE @Search
    OR u.Email LIKE @Search
    OR u.MobileNumber LIKE @Search
)";
        }

        if (request.RoleId.HasValue)
        {
            conditions += @"
AND r.Id = @RoleId";
        }

        if (request.Status.HasValue)
        {
            conditions += @"
AND u.IsActive = @Status";
        }

        var countSql = $@"
SELECT COUNT(DISTINCT u.Id)
FROM Users u
LEFT JOIN UserRoles ur ON ur.UserId = u.Id
LEFT JOIN Roles r ON r.Id = ur.RoleId
{conditions};";

        var totalCount = await connection.ExecuteScalarAsync<int>(
            countSql,
            new
            {
                HospitalId = hospitalId,
                Search = $"%{request.Search}%",
                request.RoleId,
                request.Status
            });

        var offset = (request.Page - 1) * request.PageSize;

        var sql = $@"
SELECT
    u.Id AS UserId,
    u.EmployeeCode,
    u.Username,
    CONCAT(u.FirstName, ' ', u.LastName) AS FullName,
    u.Email,
    u.MobileNumber,
    r.Name AS Role,
    u.IsActive,
    u.CreatedAt
FROM Users u
LEFT JOIN UserRoles ur
    ON ur.UserId = u.Id
LEFT JOIN Roles r
    ON r.Id = ur.RoleId
{conditions}
ORDER BY u.CreatedAt DESC
LIMIT @PageSize OFFSET @Offset;";

        var items = await connection.QueryAsync<StaffListResponse>(
            sql,
            new
            {
                HospitalId = hospitalId,
                Search = $"%{request.Search}%",
                request.RoleId,
                request.Status,
                request.PageSize,
                Offset = offset
            });

        var totalPages = request.PageSize == 0
            ? 0
            : (int)Math.Ceiling(
                totalCount / (double)request.PageSize);

        return new PagedResponse<StaffListResponse>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }

    public async Task<StaffResponse?> GetByIdAsync(
        Guid hospitalId,
        Guid userId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    u.Id AS UserId,
    u.HospitalId,
    u.EmployeeCode,
    u.Username,
    u.FirstName,
    u.LastName,
    u.Email,
    u.MobileNumber,
    r.Id AS RoleId,
    r.Name AS Role,
    u.IsActive,
    u.CreatedAt,
    u.UpdatedAt
FROM Users u
LEFT JOIN UserRoles ur
    ON ur.UserId = u.Id
LEFT JOIN Roles r
    ON r.Id = ur.RoleId
WHERE u.Id = @UserId
AND u.HospitalId = @HospitalId
AND u.IsDeleted = 0;";

        return await connection.QueryFirstOrDefaultAsync<StaffResponse>(
            sql,
            new
            {
                HospitalId = hospitalId,
                UserId = userId
            });
    }

    public async Task<User?> GetUserByIdAsync(
        Guid hospitalId,
        Guid userId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT *
FROM Users
WHERE Id = @UserId
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

        return await connection.QueryFirstOrDefaultAsync<User>(
            sql,
            new
            {
                HospitalId = hospitalId,
                UserId = userId
            });
    }

    public async Task DeleteAsync(
        Guid hospitalId,
        Guid userId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Users
SET
    IsDeleted = 1,
    IsActive = 0,
    UpdatedAt = @UpdatedAt
WHERE Id = @UserId
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                HospitalId = hospitalId,
                UserId = userId,
                UpdatedAt = DateTime.UtcNow
            });
    }

    public async Task ActivateAsync(
        Guid hospitalId,
        Guid userId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Users
SET
    IsActive = 1,
    UpdatedAt = @UpdatedAt
WHERE Id = @UserId
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                HospitalId = hospitalId,
                UserId = userId,
                UpdatedAt = DateTime.UtcNow
            });
    }

    public async Task DeactivateAsync(
        Guid hospitalId,
        Guid userId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Users
SET
    IsActive = 0,
    UpdatedAt = @UpdatedAt
WHERE Id = @UserId
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                HospitalId = hospitalId,
                UserId = userId,
                UpdatedAt = DateTime.UtcNow
            });
    }
}