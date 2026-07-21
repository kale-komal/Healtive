namespace Healtive.Core.Entities;

public class PatientInsurance
{
    public Guid Id { get; set; }

    public Guid PatientId { get; set; }

    public string InsuranceCompany { get; set; } = string.Empty;

    public string PolicyNumber { get; set; } = string.Empty;

    public string? PolicyHolderName { get; set; }

    public DateOnly? ValidFrom { get; set; }

    public DateOnly? ValidTo { get; set; }

    public decimal? CoverageAmount { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}