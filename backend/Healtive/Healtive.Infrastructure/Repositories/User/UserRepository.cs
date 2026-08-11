using Dapper;
using Healtive.Application.DTOs.User;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.Users;

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _db;

    public UserRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    // ==========================================
    // Existing authentication methods
    // ==========================================

    public async Task<User?> GetByUsernameOrEmailAsync(
        string usernameOrEmail)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
            SELECT *
            FROM Users
            WHERE (Username = @UsernameOrEmail
                OR Email = @UsernameOrEmail)
              AND IsDeleted = 0;
        ";

        return await connection.QueryFirstOrDefaultAsync<User>(
            sql,
            new
            {
                UsernameOrEmail = usernameOrEmail
            });
    }


    public async Task<User?> GetByIdAsync(Guid id)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
            SELECT *
            FROM Users
            WHERE Id = @Id
              AND IsDeleted = 0;
        ";

        return await connection.QueryFirstOrDefaultAsync<User>(
            sql,
            new { Id = id });
    }


    public async Task<IEnumerable<string>> GetUserRolesAsync(
        Guid userId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
            SELECT r.Name
            FROM Roles r
            INNER JOIN UserRoles ur
                ON r.Id = ur.RoleId
            WHERE ur.UserId = @UserId
              AND r.IsDeleted = 0
              AND r.IsActive = 1;
        ";

        return await connection.QueryAsync<string>(
            sql,
            new { UserId = userId });
    }


    public async Task UpdateLastLoginAsync(Guid userId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
            UPDATE Users
            SET LastLoginAt = @LastLoginAt
            WHERE Id = @Id;
        ";

        await connection.ExecuteAsync(
            sql,
            new
            {
                Id = userId,
                LastLoginAt = DateTime.UtcNow
            });
    }


    public async Task ChangePasswordAsync(
        Guid userId,
        string passwordHash)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
            UPDATE Users
            SET PasswordHash = @PasswordHash,
                UpdatedAt = @UpdatedAt
            WHERE Id = @Id;
        ";

        await connection.ExecuteAsync(
            sql,
            new
            {
                Id = userId,
                PasswordHash = passwordHash,
                UpdatedAt = DateTime.UtcNow
            });
    }


    // ==========================================
    // Super Admin - User List
    // ==========================================

    public async Task<IEnumerable<UserListResponse>> GetAllAsync()
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
            SELECT
                u.Id,
                u.HospitalId,

                h.Name AS HospitalName,

                u.Username,

                CONCAT(
                    u.FirstName,
                    ' ',
                    u.LastName
                ) AS Name,

                r.Name AS RoleName,

                u.Email,
                u.MobileNumber,

                u.IsActive,
                u.CreatedAt

            FROM Users u

            INNER JOIN Hospitals h
                ON u.HospitalId = h.Id

            LEFT JOIN UserRoles ur
                ON u.Id = ur.UserId

            LEFT JOIN Roles r
                ON ur.RoleId = r.Id

            WHERE u.IsDeleted = 0

            ORDER BY u.CreatedAt DESC;
        ";

        return await connection.QueryAsync<UserListResponse>(
            sql);
    }
}