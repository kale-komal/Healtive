using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Auth;

namespace Healtive.Application.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<LoginResponse>> LoginAsync(
        LoginRequest request);

    Task<LoginResponse> RefreshTokenAsync(
        string refreshToken);

    Task LogoutAsync(
        string refreshToken);

    Task<CurrentUserDto> GetCurrentUserAsync(
        Guid userId);

    Task<ApiResponse<bool>> ChangePasswordAsync(
        Guid userId,
        ChangePasswordRequest request);
}