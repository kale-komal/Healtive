namespace Healtive.Application.DTOs.User;

public class ProfileResponse
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string MobileNumber { get; set; } = string.Empty;

    public string? EmployeeCode { get; set; }

    public string? ProfileImageUrl { get; set; }

    public List<string> Roles { get; set; } = new();

    public bool IsEmailVerified { get; set; }

    public bool IsMobileVerified { get; set; }

    public bool IsActive { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public DateTime CreatedAt { get; set; }
}