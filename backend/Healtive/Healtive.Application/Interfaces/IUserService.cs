using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.User;

namespace Healtive.Application.Interfaces;

public interface IUserService
{
    Task<ApiResponse<IEnumerable<UserListResponse>>> GetAllAsync();

    Task<ApiResponse<UserViewResponse>> GetByIdAsync(Guid id);

    Task<ApiResponse<ProfileResponse>> GetProfileAsync(Guid userId);
}