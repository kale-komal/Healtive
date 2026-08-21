namespace Healtive.Application.DTOs.Doctor;

public class DoctorFilterRequest
{
    public string? Search { get; set; }

    public bool? Status { get; set; }

    public bool? Available { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}