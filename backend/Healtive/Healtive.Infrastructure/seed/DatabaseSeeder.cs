using System.Data;
using Dapper;
using Healtive.Application.Interfaces;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Seed;

public class DatabaseSeeder : IDatabaseSeeder
{
    private readonly IDbConnectionFactory _db;
    private readonly IPasswordHasher _passwordHasher;

    public DatabaseSeeder(
        IDbConnectionFactory db,
        IPasswordHasher passwordHasher)
    {
        _db = db;
        _passwordHasher = passwordHasher;
    }
    private async Task SeedSuperAdminRoleAsync(IDbConnection connection)
    {
        const string checkSql = @"
SELECT COUNT(*)
FROM Roles
WHERE Name = 'SuperAdmin'
AND HospitalId IS NULL;";

        var exists = await connection.ExecuteScalarAsync<int>(checkSql);

        if (exists > 0)
            return;

        const string insertSql = @"
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
    NULL,
    @Name,
    @Description,
    @IsSystemRole,
    @IsActive,
    @CreatedAt,
    @IsDeleted
);";

        await connection.ExecuteAsync(insertSql, new
        {
            Id = Guid.NewGuid(),
            Name = "SuperAdmin",
            Description = "System Super Administrator",
            IsSystemRole = true,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        });
    }

    private async Task SeedSuperAdminUserAsync(IDbConnection connection)
    {
        const string checkSql = @"
SELECT COUNT(*)
FROM Users
WHERE Username = 'admin';";

        var exists = await connection.ExecuteScalarAsync<int>(checkSql);

        if (exists > 0)
            return;

        var passwordHash = _passwordHasher.HashPassword("Admin@123");

        const string insertSql = @"
INSERT INTO Users
(
    Id,
    HospitalId,
    BranchId,
    EmployeeCode,
    Username,
    FirstName,
    LastName,
    Email,
    MobileNumber,
    PasswordHash,
    ProfileImageUrl,
    IsEmailVerified,
    IsMobileVerified,
    LastLoginAt,
    IsActive,
    CreatedAt,
    UpdatedAt,
    IsDeleted
)
VALUES
(
    @Id,
    NULL,
    NULL,
    @EmployeeCode,
    @Username,
    @FirstName,
    @LastName,
    @Email,
    @MobileNumber,
    @PasswordHash,
    NULL,
    TRUE,
    TRUE,
    NULL,
    TRUE,
    @CreatedAt,
    NULL,
    FALSE
);";

        await connection.ExecuteAsync(insertSql, new
        {
            Id = Guid.NewGuid(),
            EmployeeCode = "SA001",
            Username = "admin",
            FirstName = "Super",
            LastName = "Admin",
            Email = "admin@healtive.com",
            MobileNumber = "9999999999",
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow
        });
    }

    private async Task AssignSuperAdminRoleAsync(IDbConnection connection)
    {
        const string getUserSql = @"
SELECT Id
FROM Users
WHERE Username = 'admin';";

        var userId = await connection.ExecuteScalarAsync<Guid>(getUserSql);

        const string getRoleSql = @"
SELECT Id
FROM Roles
WHERE Name = 'SuperAdmin'
AND HospitalId IS NULL;";

        var roleId = await connection.ExecuteScalarAsync<Guid>(getRoleSql);

        const string checkSql = @"
SELECT COUNT(*)
FROM UserRoles
WHERE UserId = @UserId
AND RoleId = @RoleId;";

        var exists = await connection.ExecuteScalarAsync<int>(checkSql, new
        {
            UserId = userId,
            RoleId = roleId
        });

        if (exists > 0)
            return;

        const string insertSql = @"
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

        await connection.ExecuteAsync(insertSql, new
        {
            UserId = userId,
            RoleId = roleId,
            AssignedAt = DateTime.UtcNow
        });
    }
    public async Task SeedAsync()
    {
        using var connection = _db.CreateConnection();

        connection.Open();

        await SeedSuperAdminRoleAsync(connection);

        await SeedSuperAdminUserAsync(connection);

        await AssignSuperAdminRoleAsync(connection);
    }
}