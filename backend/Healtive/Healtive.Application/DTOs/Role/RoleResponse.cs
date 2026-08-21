namespace Healtive.Application.DTOs.Role;

public class RoleResponse
{
    public Guid RoleId { get; set; }

    public Guid HospitalId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsSystemRole { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}