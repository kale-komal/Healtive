using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Doctor.Lab;
using Healtive.Application.Interfaces;
using Healtive.Application.Interfaces.Repositories;
using Healtive.Core.Entities;

namespace Healtive.Infrastructure.Services.Doctors;

public class DoctorLabService : IDoctorLabService
{
    private static readonly string[] AllowedStatuses =
    {
        "Pending",
        "InProgress",
        "Completed",
        "Cancelled"
    };

    private readonly IDoctorLabRepository _repository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly ICurrentUserService _currentUserService;

    public DoctorLabService(
        IDoctorLabRepository repository,
        IDoctorRepository doctorRepository,
        ICurrentUserService currentUserService)
    {
        _repository = repository;
        _doctorRepository = doctorRepository;
        _currentUserService = currentUserService;
    }

    // =========================================================
    // GET ACTIVE LAB CATEGORIES
    // =========================================================

    public async Task<ApiResponse<IEnumerable<LabCategoryResponse>>>
        GetActiveCategoriesAsync()
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<IEnumerable<LabCategoryResponse>>
                .FailureResponse("Invalid user or hospital information.");
        }

        var doctor = await GetDoctorAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<IEnumerable<LabCategoryResponse>>
                .FailureResponse("Doctor profile not found.");
        }

        var categories = await _repository.GetActiveCategoriesAsync();

        return ApiResponse<IEnumerable<LabCategoryResponse>>
            .SuccessResponse(categories);
    }

    // =========================================================
    // GET ACTIVE LAB TESTS
    // =========================================================

    public async Task<ApiResponse<IEnumerable<LabTestResponse>>>
        GetActiveTestsAsync(
            Guid? categoryId)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<IEnumerable<LabTestResponse>>
                .FailureResponse("Invalid user or hospital information.");
        }

        var doctor = await GetDoctorAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<IEnumerable<LabTestResponse>>
                .FailureResponse("Doctor profile not found.");
        }

        var tests = await _repository.GetActiveTestsAsync(categoryId);

        return ApiResponse<IEnumerable<LabTestResponse>>
            .SuccessResponse(tests);
    }

    // =========================================================
    // GET LAB ORDERS BY APPOINTMENT
    // =========================================================

    public async Task<ApiResponse<IEnumerable<LabOrderResponse>>>
        GetByAppointmentIdAsync(
            Guid appointmentId)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<IEnumerable<LabOrderResponse>>
                .FailureResponse("Invalid user or hospital information.");
        }

        if (appointmentId == Guid.Empty)
        {
            return ApiResponse<IEnumerable<LabOrderResponse>>
                .FailureResponse("Invalid appointment ID.");
        }

        var doctor = await GetDoctorAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<IEnumerable<LabOrderResponse>>
                .FailureResponse("Doctor profile not found.");
        }

        var orders = await _repository.GetByAppointmentIdAsync(
            hospitalId,
            doctor.Id,
            appointmentId);

        return ApiResponse<IEnumerable<LabOrderResponse>>
            .SuccessResponse(orders);
    }

    // =========================================================
    // GET LAB ORDER BY ID
    // =========================================================

    public async Task<ApiResponse<LabOrderResponse>>
        GetByIdAsync(
            Guid orderId)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<LabOrderResponse>
                .FailureResponse("Invalid user or hospital information.");
        }

        if (orderId == Guid.Empty)
        {
            return ApiResponse<LabOrderResponse>
                .FailureResponse("Invalid lab order ID.");
        }

        var doctor = await GetDoctorAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<LabOrderResponse>
                .FailureResponse("Doctor profile not found.");
        }

        var order = await _repository.GetByIdAsync(
            hospitalId,
            doctor.Id,
            orderId);

        if (order == null)
        {
            return ApiResponse<LabOrderResponse>
                .FailureResponse("Lab order not found.");
        }

        return ApiResponse<LabOrderResponse>
            .SuccessResponse(order);
    }

    // =========================================================
    // CREATE LAB ORDERS
    // =========================================================

    public async Task<ApiResponse<IEnumerable<LabOrderResponse>>>
        CreateAsync(
            CreateLabOrderRequest request)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<IEnumerable<LabOrderResponse>>
                .FailureResponse("Invalid user or hospital information.");
        }

        if (request.AppointmentId == Guid.Empty)
        {
            return ApiResponse<IEnumerable<LabOrderResponse>>
                .FailureResponse("AppointmentId is required.");
        }

        var testsError = ValidateTestIds(request.TestIds);

        if (testsError != null)
        {
            return ApiResponse<IEnumerable<LabOrderResponse>>
                .FailureResponse(testsError);
        }

        var doctor = await GetDoctorAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<IEnumerable<LabOrderResponse>>
                .FailureResponse("Doctor profile not found.");
        }

        try
        {
            var orders = await _repository.CreateAsync(
                hospitalId,
                doctor.Id,
                request);

            return ApiResponse<IEnumerable<LabOrderResponse>>
                .SuccessResponse(
                    orders,
                    "Lab order created successfully.");
        }
        catch (InvalidOperationException ex)
        {
            return ApiResponse<IEnumerable<LabOrderResponse>>
                .FailureResponse(ex.Message);
        }
    }

    // =========================================================
    // UPDATE LAB ORDER
    // =========================================================

    public async Task<ApiResponse<LabOrderResponse>>
        UpdateAsync(
            Guid orderId,
            UpdateLabOrderRequest request)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<LabOrderResponse>
                .FailureResponse("Invalid user or hospital information.");
        }

        if (orderId == Guid.Empty)
        {
            return ApiResponse<LabOrderResponse>
                .FailureResponse("Invalid lab order ID.");
        }

        var statusError = ValidateStatus(request.Status);

        if (statusError != null)
        {
            return ApiResponse<LabOrderResponse>
                .FailureResponse(statusError);
        }

        var doctor = await GetDoctorAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<LabOrderResponse>
                .FailureResponse("Doctor profile not found.");
        }

        var existing = await _repository.GetByIdAsync(
            hospitalId,
            doctor.Id,
            orderId);

        if (existing == null)
        {
            return ApiResponse<LabOrderResponse>
                .FailureResponse("Lab order not found.");
        }

        // =====================================================
        // COMPLETED OR CANCELLED LAB ORDERS ARE READ-ONLY
        // =====================================================

        if (existing.Status == "Completed")
        {
            return ApiResponse<LabOrderResponse>
                .FailureResponse("Completed lab order cannot be modified.");
        }

        if (existing.Status == "Cancelled")
        {
            return ApiResponse<LabOrderResponse>
                .FailureResponse("Cancelled lab order cannot be modified.");
        }

        try
        {
            var updated = await _repository.UpdateAsync(
                hospitalId,
                doctor.Id,
                orderId,
                request);

            if (!updated)
            {
                return ApiResponse<LabOrderResponse>
                    .FailureResponse("Unable to update lab order.");
            }

            var order = await _repository.GetByIdAsync(
                hospitalId,
                doctor.Id,
                orderId);

            return ApiResponse<LabOrderResponse>
                .SuccessResponse(
                    order!,
                    "Lab order updated successfully.");
        }
        catch (InvalidOperationException ex)
        {
            return ApiResponse<LabOrderResponse>
                .FailureResponse(ex.Message);
        }
    }

    // =========================================================
    // CANCEL LAB ORDER
    // =========================================================

    public async Task<ApiResponse<string>>
        CancelAsync(
            Guid orderId)
    {
        var hospitalId = _currentUserService.HospitalId;
        var userId = _currentUserService.UserId;

        if (hospitalId == Guid.Empty || userId == Guid.Empty)
        {
            return ApiResponse<string>
                .FailureResponse("Invalid user or hospital information.");
        }

        if (orderId == Guid.Empty)
        {
            return ApiResponse<string>
                .FailureResponse("Invalid lab order ID.");
        }

        var doctor = await GetDoctorAsync(
            hospitalId,
            userId);

        if (doctor == null)
        {
            return ApiResponse<string>
                .FailureResponse("Doctor profile not found.");
        }

        var existing = await _repository.GetByIdAsync(
            hospitalId,
            doctor.Id,
            orderId);

        if (existing == null)
        {
            return ApiResponse<string>
                .FailureResponse("Lab order not found.");
        }

        // =====================================================
        // COMPLETED LAB ORDERS CANNOT BE CANCELLED
        // =====================================================

        if (existing.Status == "Completed")
        {
            return ApiResponse<string>
                .FailureResponse("Completed lab order cannot be cancelled.");
        }

        // =====================================================
        // ALREADY CANCELLED
        // =====================================================

        if (existing.Status == "Cancelled")
        {
            return ApiResponse<string>
                .FailureResponse("Lab order is already cancelled.");
        }

        var cancelled = await _repository.CancelAsync(
            hospitalId,
            doctor.Id,
            orderId);

        if (!cancelled)
        {
            return ApiResponse<string>
                .FailureResponse("Unable to cancel lab order.");
        }

        return ApiResponse<string>
            .SuccessResponse("Lab order cancelled successfully.");
    }

    // =========================================================
    // GET DOCTOR
    // =========================================================

    private async Task<Doctor?>
        GetDoctorAsync(
            Guid hospitalId,
            Guid userId)
    {
        return await _doctorRepository.GetByUserIdAsync(
            hospitalId,
            userId);
    }

    // =========================================================
    // VALIDATE TEST IDS
    // =========================================================

    private static string? ValidateTestIds(
        List<Guid>? testIds)
    {
        if (testIds == null || testIds.Count == 0)
        {
            return "At least one lab test is required.";
        }

        for (var i = 0; i < testIds.Count; i++)
        {
            if (testIds[i] == Guid.Empty)
            {
                return $"Invalid lab test selected for item {i + 1}.";
            }
        }

        var distinctCount = testIds
            .Distinct()
            .Count();

        if (distinctCount != testIds.Count)
        {
            return "Duplicate lab test selected.";
        }

        return null;
    }

    // =========================================================
    // VALIDATE STATUS
    // =========================================================

    private static string? ValidateStatus(
        string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            return "Lab order status is required.";
        }

        if (!AllowedStatuses.Contains(status))
        {
            return "Invalid lab order status.";
        }

        return null;
    }
}