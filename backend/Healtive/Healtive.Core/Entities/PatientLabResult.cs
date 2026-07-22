namespace Healtive.Core.Entities;

public class PatientLabResult
{
    public Guid Id { get; set; }

    public Guid PatientLabOrderId { get; set; }

    public string? ResultValue { get; set; }

    public string? ResultFileUrl { get; set; }

    public string? Remarks { get; set; }

    public Guid? PerformedByUserId { get; set; }

    public DateTime ResultDate { get; set; }
}