using Healtive.Core.Entities;

namespace Healtive.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail);
    Task<User?> GetByIdAsync(Guid id);

    Task<IEnumerable<string>> GetUserRolesAsync(Guid userId);

    Task UpdateLastLoginAsync(Guid userId);

    Task ChangePasswordAsync(Guid userId, string passwordHash);
}