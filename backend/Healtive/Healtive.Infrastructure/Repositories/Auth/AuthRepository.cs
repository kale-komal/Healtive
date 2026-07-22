using Dapper;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.Auth;

public class AuthRepository : IAuthRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public AuthRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task SaveRefreshTokenAsync(UserRefreshToken refreshToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
        INSERT INTO UserRefreshTokens
        (
            Id,
            UserId,
            RefreshToken,
            ExpiresAt,
            IsRevoked,
            CreatedAt,
            RevokedAt
        )
        VALUES
        (
            @Id,
            @UserId,
            @RefreshToken,
            @ExpiresAt,
            @IsRevoked,
            @CreatedAt,
            @RevokedAt
        );";

        await connection.ExecuteAsync(sql, refreshToken);
    }

    public async Task<UserRefreshToken?> GetRefreshTokenAsync(string refreshToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
        SELECT
            Id,
            UserId,
            RefreshToken,
            ExpiresAt,
            IsRevoked,
            CreatedAt,
            RevokedAt
        FROM UserRefreshTokens
        WHERE RefreshToken = @RefreshToken
        LIMIT 1;";

        return await connection.QuerySingleOrDefaultAsync<UserRefreshToken>(
            sql,
            new
            {
                RefreshToken = refreshToken
            });
    }

    public async Task RevokeRefreshTokenAsync(Guid tokenId)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
        UPDATE UserRefreshTokens
        SET
            IsRevoked = TRUE,
            RevokedAt = CURRENT_TIMESTAMP
        WHERE Id = @TokenId;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                TokenId = tokenId
            });
    }

    public async Task AddLoginHistoryAsync(UserLoginHistory loginHistory)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        const string sql = @"
        INSERT INTO UserLoginHistory
        (
            Id,
            UserId,
            LoginTime,
            LogoutTime,
            IpAddress,
            Device,
            Browser,
            OperatingSystem,
            IsSuccessful
        )
        VALUES
        (
            @Id,
            @UserId,
            @LoginTime,
            @LogoutTime,
            @IpAddress,
            @Device,
            @Browser,
            @OperatingSystem,
            @IsSuccessful
        );";

        await connection.ExecuteAsync(sql, loginHistory);
    }
}