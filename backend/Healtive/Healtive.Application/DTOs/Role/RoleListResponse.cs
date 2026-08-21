namespace Healtive.Application.DTOs.Role;

public class RoleListResponse
{
    public Guid RoleId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsSystemRole { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}