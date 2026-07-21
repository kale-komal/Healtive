namespace Healtive.Core.Entities;

public class DoctorDepartment
{
    public Guid DoctorId { get; set; }

    public Guid DepartmentId { get; set; }

    public DateTime CreatedAt { get; set; }
}