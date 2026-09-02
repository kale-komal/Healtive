namespace Healtive.Application.DTOs.Doctor.PatientMedicalHistory;

public class DoctorPatientMedicalHistorySummaryResponse
{
    public Guid PatientId { get; set; }

    public string PatientCode { get; set; } = string.Empty;

    public string PatientName { get; set; } = string.Empty;

    public int TotalVisits { get; set; }

    public DateOnly? LastVisitDate { get; set; }

    public DateOnly? FirstVisitDate { get; set; }
}