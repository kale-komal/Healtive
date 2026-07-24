namespace Healtive.Application.DTOs.Hospital;

public class HospitalResponse
{
    public Guid HospitalId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string HospitalName { get; set; } = string.Empty;

    public string AdminUsername { get; set; } = string.Empty;

    public string TemporaryPassword { get; set; } = string.Empty;

    public string PlanName { get; set; } = string.Empty;
}