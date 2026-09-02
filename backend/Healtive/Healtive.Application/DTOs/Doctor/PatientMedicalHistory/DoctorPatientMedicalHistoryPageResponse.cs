using Healtive.Application.DTOs.Common;

namespace Healtive.Application.DTOs.Doctor.PatientMedicalHistory;

public class DoctorPatientMedicalHistoryPageResponse
{
    public DoctorPatientMedicalHistorySummaryResponse Summary { get; set; }
        = new();

    public PagedResponse<DoctorPatientMedicalHistoryResponse> History { get; set; }
        = new();
}