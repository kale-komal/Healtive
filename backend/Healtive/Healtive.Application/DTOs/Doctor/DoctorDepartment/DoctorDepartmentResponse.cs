namespace Healtive.Application.DTOs.Doctor;

public class DoctorDepartmentResponse
{
    public Guid DoctorId { get; set; }

    public Guid DepartmentId { get; set; }

    public string DepartmentName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}