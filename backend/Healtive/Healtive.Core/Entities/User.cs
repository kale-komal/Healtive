namespace Healtive.Core.Entities;

public class User
{
    public Guid Id { get; set; }

    public Guid HospitalId { get; set; }

    public Guid? BranchId { get; set; }

    public string? EmployeeCode { get; set; }

    public string Username { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string MobileNumber { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string? ProfileImageUrl { get; set; }

    public bool IsEmailVerified { get; set; }

    public bool IsMobileVerified { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiry { get; set; }

    public DateTime? LastPasswordChangedAt { get; set; }

    public int FailedLoginAttempts { get; set; }

    public DateTime? LockoutEnd { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }
}