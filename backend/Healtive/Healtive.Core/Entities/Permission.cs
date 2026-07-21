namespace Healtive.Core.Entities;

public class Permission
{
    public Guid Id { get; set; }

    public string Module { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}