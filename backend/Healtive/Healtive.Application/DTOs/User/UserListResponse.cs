namespace Healtive.Application.DTOs.User;

public class UserListResponse
{
    public Guid Id { get; set; }

    public Guid HospitalId { get; set; }

    public string HospitalName { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string MobileNumber { get; set; } = string.Empty;

    public string? RoleName { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}