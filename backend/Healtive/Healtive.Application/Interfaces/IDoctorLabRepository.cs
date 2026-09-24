using Healtive.Application.DTOs.Doctor.Lab;

namespace Healtive.Application.Interfaces.Repositories;

public interface IDoctorLabRepository
{
    Task<IEnumerable<LabCategoryResponse>> GetActiveCategoriesAsync();

    Task<IEnumerable<LabTestResponse>> GetActiveTestsAsync(
        Guid? categoryId);

    Task<IEnumerable<LabOrderResponse>> GetByAppointmentIdAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid appointmentId);

    Task<LabOrderResponse?> GetByIdAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid orderId);

    Task<IEnumerable<LabOrderResponse>> CreateAsync(
        Guid hospitalId,
        Guid doctorId,
        CreateLabOrderRequest request);

    Task<bool> UpdateAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid orderId,
        UpdateLabOrderRequest request);

    Task<bool> CancelAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid orderId);
}