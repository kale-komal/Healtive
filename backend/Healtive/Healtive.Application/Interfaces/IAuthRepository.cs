using Healtive.Core.Entities;

namespace Healtive.Application.Interfaces;

public interface IAuthRepository
{
    Task SaveRefreshTokenAsync(UserRefreshToken refreshToken);

    Task<UserRefreshToken?> GetRefreshTokenAsync(string refreshToken);

    Task RevokeRefreshTokenAsync(Guid tokenId);

    Task AddLoginHistoryAsync(UserLoginHistory loginHistory);
}