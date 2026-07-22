using Healtive.Core.Entities;

namespace Healtive.Application.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(User user, IEnumerable<string> roles);

    string GenerateRefreshToken();

    DateTime GetRefreshTokenExpiry();
}