using Healtive.Application.DTOs.Auth;
using Healtive.Application.DTOs.Common;


namespace Healtive.Application.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request);

    Task<LoginResponse> RefreshTokenAsync(string refreshToken);

    Task LogoutAsync(string refreshToken);

    Task<CurrentUserDto> GetCurrentUserAsync(Guid userId);
}