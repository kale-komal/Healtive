using Healtive.Application.DTOs.Branch;
using Healtive.Application.DTOs.Common;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;

namespace Healtive.Infrastructure.Services.Branches;

public class BranchService : IBranchService
{
    private readonly IBranchRepository _branchRepository;
    private readonly ICurrentUserService _currentUserService;

    public BranchService(
        IBranchRepository branchRepository,
        ICurrentUserService currentUserService)
    {
        _branchRepository = branchRepository;
        _currentUserService = currentUserService;
    }

    public async Task<ApiResponse<string>> CreateAsync(
        CreateBranchRequest request)
    {
        var hospitalId = _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<string>.FailureResponse(
                "Hospital information not found.");
        }

        // Check duplicate branch code
        if (await _branchRepository.ExistsByCodeAsync(
            hospitalId,
            request.Code))
        {
            return ApiResponse<string>.FailureResponse(
                "Branch code already exists.");
        }

        // If this branch is going to be Head Office,
        // remove Head Office status from existing branches.
        if (request.IsHeadOffice)
        {
            await _branchRepository.ClearHeadOfficeAsync(hospitalId);
        }

        var branch = new Branch
        {
            Id = Guid.NewGuid(),

            HospitalId = hospitalId,

            Name = request.Name,
            Code = request.Code,

            Email = request.Email,
            PhoneNumber = request.PhoneNumber,

            Address = request.Address,
            City = request.City,
            State = request.State,
            Country = request.Country,
            PostalCode = request.PostalCode,

            IsHeadOffice = request.IsHeadOffice,

            IsActive = true,

            CreatedAt = DateTime.UtcNow,

            IsDeleted = false
        };

        await _branchRepository.CreateAsync(branch);

        return ApiResponse<string>.SuccessResponse(
            "Branch created successfully.",
            "Success");
    }

    public async Task<ApiResponse<PagedResponse<BranchListResponse>>> GetAllAsync(
        string? search,
        bool? status,
        int page,
        int pageSize)
    {
        var hospitalId = _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<PagedResponse<BranchListResponse>>
                .FailureResponse(
                    "Hospital information not found.");
        }

        if (page < 1)
            page = 1;

        if (pageSize < 1)
            pageSize = 10;

        if (pageSize > 100)
            pageSize = 100;

        var result = await _branchRepository.GetAllAsync(
            hospitalId,
            search,
            status,
            page,
            pageSize);

        return ApiResponse<PagedResponse<BranchListResponse>>
            .SuccessResponse(
                result,
                "Branches fetched successfully.");
    }

    public async Task<ApiResponse<BranchResponse>> GetByIdAsync(
        Guid branchId)
    {
        var hospitalId = _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<BranchResponse>.FailureResponse(
                "Hospital information not found.");
        }

        var branch = await _branchRepository.GetByIdAsync(
            hospitalId,
            branchId);

        if (branch == null)
        {
            return ApiResponse<BranchResponse>.FailureResponse(
                "Branch not found.");
        }

        var response = new BranchResponse
        {
            BranchId = branch.Id,
            HospitalId = branch.HospitalId,

            Name = branch.Name,
            Code = branch.Code,

            Email = branch.Email,
            PhoneNumber = branch.PhoneNumber,

            Address = branch.Address,
            City = branch.City,
            State = branch.State,
            Country = branch.Country,
            PostalCode = branch.PostalCode,

            IsHeadOffice = branch.IsHeadOffice,
            IsActive = branch.IsActive,

            CreatedAt = branch.CreatedAt,
            UpdatedAt = branch.UpdatedAt
        };

        return ApiResponse<BranchResponse>.SuccessResponse(
            response,
            "Branch fetched successfully.");
    }

    public async Task<ApiResponse<string>> UpdateAsync(
        Guid branchId,
        UpdateBranchRequest request)
    {
        var hospitalId = _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<string>.FailureResponse(
                "Hospital information not found.");
        }

        var branch = await _branchRepository.GetByIdAsync(
            hospitalId,
            branchId);

        if (branch == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Branch not found.");
        }

        // Check duplicate code excluding current branch
        if (await _branchRepository.ExistsByCodeAsync(
            hospitalId,
            branchId,
            request.Code))
        {
            return ApiResponse<string>.FailureResponse(
                "Branch code already exists.");
        }

        // If this branch becomes Head Office,
        // clear the previous Head Office.
        if (request.IsHeadOffice &&
            !branch.IsHeadOffice)
        {
            await _branchRepository.ClearHeadOfficeAsync(
                hospitalId);
        }

        branch.Name = request.Name;
        branch.Code = request.Code;

        branch.Email = request.Email;
        branch.PhoneNumber = request.PhoneNumber;

        branch.Address = request.Address;
        branch.City = request.City;
        branch.State = request.State;
        branch.Country = request.Country;
        branch.PostalCode = request.PostalCode;

        branch.IsHeadOffice = request.IsHeadOffice;

        branch.UpdatedAt = DateTime.UtcNow;

        await _branchRepository.UpdateAsync(branch);

        return ApiResponse<string>.SuccessResponse(
            "Branch updated successfully.",
            "Success");
    }

    public async Task<ApiResponse<string>> DeleteAsync(
        Guid branchId)
    {
        var hospitalId = _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<string>.FailureResponse(
                "Hospital information not found.");
        }

        var branch = await _branchRepository.GetByIdAsync(
            hospitalId,
            branchId);

        if (branch == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Branch not found.");
        }

        await _branchRepository.DeleteAsync(
            hospitalId,
            branchId);

        return ApiResponse<string>.SuccessResponse(
            "Branch deleted successfully.",
            "Success");
    }

    public async Task<ApiResponse<string>> ActivateAsync(
        Guid branchId)
    {
        var hospitalId = _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<string>.FailureResponse(
                "Hospital information not found.");
        }

        var branch = await _branchRepository.GetByIdAsync(
            hospitalId,
            branchId);

        if (branch == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Branch not found.");
        }

        if (branch.IsActive)
        {
            return ApiResponse<string>.FailureResponse(
                "Branch is already active.");
        }

        await _branchRepository.ActivateAsync(
            hospitalId,
            branchId);

        return ApiResponse<string>.SuccessResponse(
            "Branch activated successfully.",
            "Success");
    }

    public async Task<ApiResponse<string>> DeactivateAsync(
        Guid branchId)
    {
        var hospitalId = _currentUserService.HospitalId;

        if (hospitalId == Guid.Empty)
        {
            return ApiResponse<string>.FailureResponse(
                "Hospital information not found.");
        }

        var branch = await _branchRepository.GetByIdAsync(
            hospitalId,
            branchId);

        if (branch == null)
        {
            return ApiResponse<string>.FailureResponse(
                "Branch not found.");
        }

        if (!branch.IsActive)
        {
            return ApiResponse<string>.FailureResponse(
                "Branch is already inactive.");
        }

        await _branchRepository.DeactivateAsync(
            hospitalId,
            branchId);

        return ApiResponse<string>.SuccessResponse(
            "Branch deactivated successfully.",
            "Success");
    }
}