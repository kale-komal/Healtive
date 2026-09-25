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

    public async Task<string?> GetAssignableRoleNameAsync(
        Guid hospitalId,
        Guid roleId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT Name
FROM Roles
WHERE Id = @RoleId
AND HospitalId = @HospitalId
AND IsActive = 1
AND IsDeleted = 0
AND LOWER(Name) NOT IN ('hospitaladmin', 'superadmin', 'doctor')
LIMIT 1;";

        return await connection.QueryFirstOrDefaultAsync<string>(
            sql,
            new
            {
                HospitalId = hospitalId,
                RoleId = roleId
            });
    }

    public async Task<IEnumerable<string>> GetUserRoleNamesAsync(
        Guid hospitalId,
        Guid userId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT r.Name
FROM UserRoles ur
INNER JOIN Roles r ON r.Id = ur.RoleId
INNER JOIN Users u ON u.Id = ur.UserId
WHERE ur.UserId = @UserId
AND u.HospitalId = @HospitalId
AND r.IsDeleted = 0;";

        return await connection.QueryAsync<string>(
            sql,
            new
            {
                HospitalId = hospitalId,
                UserId = userId
            });
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
        connection.Open();

        using var transaction = connection.BeginTransaction();

        // Replace the user's existing staff-role mappings with the newly
        // selected primary staff role. Doctor, HospitalAdmin and
        // SuperAdmin mappings are never removed here.
        const string deleteSql = @"
DELETE ur
FROM UserRoles ur
INNER JOIN Roles r ON r.Id = ur.RoleId
WHERE ur.UserId = @UserId
AND r.IsDeleted = 0
AND LOWER(r.Name) NOT IN ('doctor', 'hospitaladmin', 'superadmin');";

        const string insertSql = @"
INSERT INTO UserRoles
(
    UserId,
    RoleId,
    AssignedAt
)
SELECT @UserId, @RoleId, @AssignedAt
WHERE NOT EXISTS (
    SELECT 1
    FROM UserRoles
    WHERE UserId = @UserId
    AND RoleId = @RoleId
);";

        await connection.ExecuteAsync(
            deleteSql,
            new
            {
                UserId = userId
            },
            transaction);

        await connection.ExecuteAsync(
            insertSql,
            new
            {
                UserId = userId,
                RoleId = roleId,
                AssignedAt = DateTime.UtcNow
            },
            transaction);

        transaction.Commit();
    }

    public async Task<PagedResponse<StaffListResponse>> GetAllAsync(
        StaffFilterRequest request,
        Guid hospitalId)
    {
        using var connection = _db.CreateConnection();

        var conditions = @"
WHERE u.HospitalId = @HospitalId
AND u.IsDeleted = 0
AND u.Id NOT IN (
    SELECT ur2.UserId
    FROM UserRoles ur2
    INNER JOIN Roles r2
        ON r2.Id = ur2.RoleId
    WHERE r2.IsDeleted = 0
    AND LOWER(r2.Name) IN ('doctor', 'hospitaladmin', 'superadmin')
)";

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