using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.DoctorSpecialization;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;

namespace Healtive.Infrastructure.Services.DoctorSpecializations;

public class DoctorSpecializationService
    : IDoctorSpecializationService
{
    private readonly IDoctorSpecializationRepository _repository;

    public DoctorSpecializationService(
        IDoctorSpecializationRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<DoctorSpecializationResponse>> CreateAsync(
        CreateDoctorSpecializationRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return ApiResponse<DoctorSpecializationResponse>
                .FailureResponse(
                    "Specialization name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Code))
        {
            return ApiResponse<DoctorSpecializationResponse>
                .FailureResponse(
                    "Specialization code is required.");
        }

        var name = request.Name.Trim();
        var code = request.Code.Trim().ToUpperInvariant();

        if (await _repository.ExistsByNameAsync(name))
        {
            return ApiResponse<DoctorSpecializationResponse>
                .FailureResponse(
                    "Specialization name already exists.");
        }

        if (await _repository.ExistsByCodeAsync(code))
        {
            return ApiResponse<DoctorSpecializationResponse>
                .FailureResponse(
                    "Specialization code already exists.");
        }

        var specialization = new DoctorSpecialization
        {
            Id = Guid.NewGuid(),
            Name = name,
            Code = code,
            Description = request.Description?.Trim(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        await _repository.CreateAsync(specialization);

        var response =
            await _repository.GetByIdAsync(
                specialization.Id);

        if (response == null)
        {
            return ApiResponse<DoctorSpecializationResponse>
                .FailureResponse(
                    "Specialization created but could not be retrieved.");
        }

        return ApiResponse<DoctorSpecializationResponse>
            .SuccessResponse(
                response,
                "Doctor specialization created successfully.");
    }

    public async Task<ApiResponse<PagedResponse<DoctorSpecializationResponse>>>
        GetAllAsync(
            DoctorSpecializationFilterRequest request)
    {
        if (request.Page < 1)
        {
            request.Page = 1;
        }

        if (request.PageSize < 1)
        {
            request.PageSize = 10;
        }

        if (request.PageSize > 100)
        {
            request.PageSize = 100;
        }

        var result =
            await _repository.GetAllAsync(request);

        return ApiResponse<PagedResponse<DoctorSpecializationResponse>>
            .SuccessResponse(
                result,
                "Doctor specializations fetched successfully.");
    }

    public async Task<ApiResponse<DoctorSpecializationResponse>>
        GetByIdAsync(Guid id)
    {
        var specialization =
            await _repository.GetByIdAsync(id);

        if (specialization == null)
        {
            return ApiResponse<DoctorSpecializationResponse>
                .FailureResponse(
                    "Doctor specialization not found.");
        }

        return ApiResponse<DoctorSpecializationResponse>
            .SuccessResponse(
                specialization,
                "Doctor specialization fetched successfully.");
    }

    public async Task<ApiResponse<string>> UpdateAsync(
        Guid id,
        UpdateDoctorSpecializationRequest request)
    {
        var specialization =
            await _repository.GetEntityByIdAsync(id);

        if (specialization == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Doctor specialization not found.");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return ApiResponse<string>.FailureResponse(
                "Specialization name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Code))
        {
            return ApiResponse<string>.FailureResponse(
                "Specialization code is required.");
        }

        var name = request.Name.Trim();
        var code = request.Code.Trim().ToUpperInvariant();

        if (await _repository.ExistsByNameAsync(id, name))
        {
            return ApiResponse<string>.FailureResponse(
                "Specialization name already exists.");
        }

        if (await _repository.ExistsByCodeAsync(id, code))
        {
            return ApiResponse<string>.FailureResponse(
                "Specialization code already exists.");
        }

        specialization.Name = name;
        specialization.Code = code;
        specialization.Description =
            request.Description?.Trim();
        specialization.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(specialization);

        return ApiResponse<string>.SuccessResponse(
            "Doctor specialization updated successfully.",
            "Success");
    }

    public async Task<ApiResponse<string>> DeleteAsync(
        Guid id)
    {
        var specialization =
            await _repository.GetEntityByIdAsync(id);

        if (specialization == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Doctor specialization not found.");
        }

        await _repository.DeleteAsync(id);

        return ApiResponse<string>.SuccessResponse(
            "Doctor specialization deleted successfully.",
            "Success");
    }

    public async Task<ApiResponse<string>> ActivateAsync(
        Guid id)
    {
        var specialization =
            await _repository.GetEntityByIdAsync(id);

        if (specialization == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Doctor specialization not found.");
        }

        if (specialization.IsActive)
        {
            return ApiResponse<string>.FailureResponse(
                "Doctor specialization is already active.");
        }

        await _repository.ActivateAsync(id);

        return ApiResponse<string>.SuccessResponse(
            "Doctor specialization activated successfully.",
            "Success");
    }

    public async Task<ApiResponse<string>> DeactivateAsync(
        Guid id)
    {
        var specialization =
            await _repository.GetEntityByIdAsync(id);

        if (specialization == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Doctor specialization not found.");
        }

        if (!specialization.IsActive)
        {
            return ApiResponse<string>.FailureResponse(
                "Doctor specialization is already inactive.");
        }

        await _repository.DeactivateAsync(id);

        return ApiResponse<string>.SuccessResponse(
            "Doctor specialization deactivated successfully.",
            "Success");
    }
}