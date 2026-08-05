namespace Healtive.Application.DTOs.Common;

public class PaginationRequest
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? Search { get; set; }

    public bool? IsActive { get; set; }

    public string SortBy { get; set; } = "CreatedAt";

    public string SortDirection { get; set; } = "DESC";
}