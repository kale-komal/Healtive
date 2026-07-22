using Healtive.Application.DTOs.Auth;
using Healtive.Application.DTOs.Common;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;

namespace Healtive.Infrastructure.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public AuthService(
        IUserRepository userRepository,
        IAuthRepository authRepository,
        IPasswordHasher passwordHasher,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public Task<CurrentUserDto> GetCurrentUserAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request)
    {
        // Step 1 - Find user
        var user = await _userRepository.GetByUsernameOrEmailAsync(
            request.UsernameOrEmail);

        if (user is null)
        {
            return ApiResponse<LoginResponse>.FailureResponse(
                "Invalid username or email.");
        }

        // Step 2 - Verify password
        var isPasswordValid = _passwordHasher.VerifyPassword(
            request.Password,
            user.PasswordHash);

        if (!isPasswordValid)
        {
            var successLoginHistory = new UserLoginHistory
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                LoginTime = DateTime.UtcNow,
                IsSuccessful = true
            };

            await _authRepository.AddLoginHistoryAsync(successLoginHistory);

            return ApiResponse<LoginResponse>.FailureResponse(
    "Invalid username/email or password.");
        }

        // Step 3 - Load user roles
        var roles = (await _userRepository.GetUserRolesAsync(user.Id)).ToList();

        if (!roles.Any())
        {
            return ApiResponse<LoginResponse>.FailureResponse(
                "No role assigned to this user.");
        }

        // Step 4 - Generate Access Token
        var accessToken = _jwtService.GenerateAccessToken(
            user,
            roles);

        // Step 5 - Generate Refresh Token
        var refreshToken = _jwtService.GenerateRefreshToken();

        var refreshTokenEntity = new UserRefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            RefreshToken = refreshToken,
            ExpiresAt = _jwtService.GetAccessTokenExpiry(),
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow
        };

        await _authRepository.SaveRefreshTokenAsync(refreshTokenEntity);

        // Step 6 - Update Last Login
        await _userRepository.UpdateLastLoginAsync(user.Id);

        // Step 7 - Save Successful Login History
        var loginHistory = new UserLoginHistory
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            LoginTime = DateTime.UtcNow,
            IsSuccessful = true
        };

        await _authRepository.AddLoginHistoryAsync(loginHistory);

        // Step 8 - Build Response
        var response = new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(120), // We'll improve this next
            User = new CurrentUserDto
            {
                UserId = user.Id,
                HospitalId = user.HospitalId,
                BranchId = user.BranchId,
                Username = user.Username,
                FullName = $"{user.FirstName} {user.LastName}",
                Role = roles.First()
            }
        };

        return ApiResponse<LoginResponse>.SuccessResponse(
            response,
            "Login successful.");
    }
    public Task LogoutAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task<LoginResponse> RefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }
}