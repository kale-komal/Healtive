namespace Healtive.Application.DTOs.Doctor.Lab;

public class UpdateLabOrderRequest
{
    public string? Remarks { get; set; }

    public string Status { get; set; } = string.Empty;
}