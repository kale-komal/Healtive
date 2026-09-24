using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Doctor.Lab;

namespace Healtive.Application.Interfaces;

public interface IDoctorLabService
{
    Task<ApiResponse<IEnumerable<LabCategoryResponse>>> GetActiveCategoriesAsync();

    Task<ApiResponse<IEnumerable<LabTestResponse>>> GetActiveTestsAsync(
        Guid? categoryId);

    Task<ApiResponse<IEnumerable<LabOrderResponse>>> GetByAppointmentIdAsync(
        Guid appointmentId);

    Task<ApiResponse<LabOrderResponse>> GetByIdAsync(
        Guid orderId);

    Task<ApiResponse<IEnumerable<LabOrderResponse>>> CreateAsync(
        CreateLabOrderRequest request);

    Task<ApiResponse<LabOrderResponse>> UpdateAsync(
        Guid orderId,
        UpdateLabOrderRequest request);

    Task<ApiResponse<string>> CancelAsync(
        Guid orderId);
}