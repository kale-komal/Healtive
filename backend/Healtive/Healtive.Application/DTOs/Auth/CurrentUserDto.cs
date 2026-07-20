namespace Healtive.Application.DTOs.Auth;

public class CurrentUserDto
{
    public Guid UserId { get; set; }

    public Guid HospitalId { get; set; }

    public Guid? BranchId { get; set; }

    public string Username { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
}