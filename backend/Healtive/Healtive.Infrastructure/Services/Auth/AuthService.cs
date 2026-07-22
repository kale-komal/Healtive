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
            var loginHistory = new UserLoginHistory
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                LoginTime = DateTime.UtcNow,
                IsSuccessful = false
            };

            await _authRepository.AddLoginHistoryAsync(loginHistory);

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

        throw new NotImplementedException();
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