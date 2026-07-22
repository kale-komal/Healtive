using Dapper;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.Auth;

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public UserRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
        SELECT *
        FROM Users
        WHERE
            (
                Username = @UsernameOrEmail
                OR Email = @UsernameOrEmail
            )
        AND IsDeleted = FALSE
        AND IsActive = TRUE
        LIMIT 1;";

        return await connection.QuerySingleOrDefaultAsync<User>(
            sql,
            new
            {
                UsernameOrEmail = usernameOrEmail
            });
    }
    public async Task<User?> GetByIdAsync(Guid id)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
        SELECT *
        FROM Users
        WHERE Id = @Id
          AND IsDeleted = FALSE
          AND IsActive = TRUE
        LIMIT 1;";

        return await connection.QuerySingleOrDefaultAsync<User>(
            sql,
            new
            {
                Id = id
            });
    }
    public async Task<IEnumerable<string>> GetUserRolesAsync(Guid userId)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
        SELECT r.Name
        FROM UserRoles ur
        INNER JOIN Roles r
            ON ur.RoleId = r.Id
        WHERE ur.UserId = @UserId
          AND r.IsActive = TRUE
          AND r.IsDeleted = FALSE;";

        return await connection.QueryAsync<string>(
            sql,
            new
            {
                UserId = userId
            });
    }

    public async Task UpdateLastLoginAsync(Guid userId)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
        UPDATE Users
        SET LastLoginAt = CURRENT_TIMESTAMP
        WHERE Id = @UserId;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                UserId = userId
            });
    }
    public async Task ChangePasswordAsync(Guid userId, string passwordHash)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
        UPDATE Users
        SET PasswordHash = @PasswordHash,
            LastPasswordChangedAt = CURRENT_TIMESTAMP
        WHERE Id = @UserId;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                UserId = userId,
                PasswordHash = passwordHash
            });
    }
}