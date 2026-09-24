namespace Healtive.Application.DTOs.Doctor.Lab;

public class LabOrderResponse
{
    public Guid Id { get; set; }

    public string OrderNumber { get; set; } = string.Empty;

    public Guid PatientId { get; set; }

    public Guid? AppointmentId { get; set; }

    public Guid DoctorId { get; set; }

    public Guid LabTestId { get; set; }

    public string LabTestName { get; set; } = string.Empty;

    public string TestCode { get; set; } = string.Empty;

    public DateTime OrderDate { get; set; }

    public string Status { get; set; } = string.Empty;

    public string? Remarks { get; set; }

    public DateTime CreatedAt { get; set; }
}