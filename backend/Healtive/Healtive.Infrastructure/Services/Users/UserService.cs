using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.User;
using Healtive.Application.Interfaces;

namespace Healtive.Infrastructure.Services.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApiResponse<IEnumerable<UserListResponse>>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return ApiResponse<IEnumerable<UserListResponse>>
            .SuccessResponse(
                users,
                "Users fetched successfully."
            );
    }

    public async Task<ApiResponse<UserViewResponse>> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            return ApiResponse<UserViewResponse>
                .FailureResponse("User not found.");
        }

        var roles = await _userRepository.GetUserRolesAsync(id);

        var response = new UserViewResponse
        {
            Id = user.Id,
            HospitalId = user.HospitalId,
            BranchId = user.BranchId,
            EmployeeCode = user.EmployeeCode,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            MobileNumber = user.MobileNumber,
            ProfileImageUrl = user.ProfileImageUrl,
            IsEmailVerified = user.IsEmailVerified,
            IsMobileVerified = user.IsMobileVerified,
            LastLoginAt = user.LastLoginAt,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            Roles = roles.ToList()
        };

        return ApiResponse<UserViewResponse>
            .SuccessResponse(
                response,
                "User fetched successfully."
            );
    }
}