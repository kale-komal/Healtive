namespace Healtive.Application.DTOs.Branch;

public class BranchListResponse
{
    public Guid BranchId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public string City { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public bool IsHeadOffice { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}