namespace Healtive.Application.DTOs.Appointment;

public class AppointmentFilterRequest
{
    public Guid? BranchId { get; set; }

    public Guid? DoctorId { get; set; }

    public Guid? PatientId { get; set; }

    public Guid? DepartmentId { get; set; }

    public Guid? AppointmentStatusId { get; set; }

    public DateOnly? FromDate { get; set; }

    public DateOnly? ToDate { get; set; }

    public string? Search { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}