namespace Healtive.Application.DTOs.Doctor.PatientMedicalHistory;

public class DoctorPatientMedicalHistoryFilterRequest
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}