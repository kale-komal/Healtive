namespace Healtive.Application.DTOs.Role;

public class RoleFilterRequest
{
    public string? Search { get; set; }

    public bool? Status { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}