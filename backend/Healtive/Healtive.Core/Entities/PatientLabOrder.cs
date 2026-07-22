namespace Healtive.Core.Entities;

public class PatientLabOrder
{
    public Guid Id { get; set; }

    public string OrderNumber { get; set; } = string.Empty;

    public Guid PatientId { get; set; }

    public Guid? AppointmentId { get; set; }

    public Guid DoctorId { get; set; }

    public Guid LabTestId { get; set; }

    public DateTime OrderDate { get; set; }

    public string Status { get; set; } = string.Empty;

    public string? Remarks { get; set; }

    public DateTime CreatedAt { get; set; }
}