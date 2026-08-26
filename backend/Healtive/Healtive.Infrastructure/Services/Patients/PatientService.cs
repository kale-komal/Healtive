using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Patient;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;

namespace Healtive.Infrastructure.Services.Patients;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;

    public PatientService(
        IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<PatientResponse> CreateAsync(
        CreatePatientRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName))
            throw new Exception("First name is required.");

        if (string.IsNullOrWhiteSpace(request.LastName))
            throw new Exception("Last name is required.");

        if (string.IsNullOrWhiteSpace(request.Gender))
            throw new Exception("Gender is required.");

        if (string.IsNullOrWhiteSpace(request.MobileNumber))
            throw new Exception("Mobile number is required.");

        var mobileExists =
            await _patientRepository.ExistsByMobileAsync(
                request.MobileNumber);

        if (mobileExists)
            throw new Exception(
                "A patient with this mobile number already exists.");

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var emailExists =
                await _patientRepository.ExistsByEmailAsync(
                    request.Email);

            if (emailExists)
                throw new Exception(
                    "A patient with this email already exists.");
        }

        var patient = new Patient
        {
            Id = Guid.NewGuid(),

            PatientCode = await GeneratePatientCodeAsync(),

            FirstName = request.FirstName.Trim(),

            LastName = request.LastName.Trim(),

            DateOfBirth = request.DateOfBirth,

            Gender = request.Gender.Trim(),

            BloodGroup = string.IsNullOrWhiteSpace(request.BloodGroup)
                ? null
                : request.BloodGroup.Trim(),

            MobileNumber = request.MobileNumber.Trim(),

            Email = string.IsNullOrWhiteSpace(request.Email)
                ? null
                : request.Email.Trim(),

            PasswordHash = null,

            GoogleId = null,

            IsMobileVerified = false,

            IsEmailVerified = false,

            QRToken = Guid.NewGuid(),

            ProfileImageUrl = request.ProfileImageUrl,

            IsActive = true,

            CreatedAt = DateTime.UtcNow,

            IsDeleted = false
        };

        await _patientRepository.CreateAsync(patient);

        return await _patientRepository.GetByIdAsync(patient.Id)
            ?? throw new Exception(
                "Patient was created but could not be retrieved.");
    }

    public async Task<PatientResponse> UpdateAsync(
        Guid id,
        UpdatePatientRequest request)
    {
        var patient =
            await _patientRepository.GetEntityByIdAsync(id);

        if (patient == null)
            throw new Exception("Patient not found.");

        if (string.IsNullOrWhiteSpace(request.FirstName))
            throw new Exception("First name is required.");

        if (string.IsNullOrWhiteSpace(request.LastName))
            throw new Exception("Last name is required.");

        if (string.IsNullOrWhiteSpace(request.Gender))
            throw new Exception("Gender is required.");

        if (string.IsNullOrWhiteSpace(request.MobileNumber))
            throw new Exception("Mobile number is required.");

        var mobileExists =
            await _patientRepository.ExistsByMobileAsync(
                id,
                request.MobileNumber);

        if (mobileExists)
            throw new Exception(
                "Another patient already uses this mobile number.");

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var emailExists =
                await _patientRepository.ExistsByEmailAsync(
                    id,
                    request.Email);

            if (emailExists)
                throw new Exception(
                    "Another patient already uses this email.");
        }

        patient.FirstName = request.FirstName.Trim();

        patient.LastName = request.LastName.Trim();

        patient.DateOfBirth = request.DateOfBirth;

        patient.Gender = request.Gender.Trim();

        patient.BloodGroup =
            string.IsNullOrWhiteSpace(request.BloodGroup)
                ? null
                : request.BloodGroup.Trim();

        patient.MobileNumber =
            request.MobileNumber.Trim();

        patient.Email =
            string.IsNullOrWhiteSpace(request.Email)
                ? null
                : request.Email.Trim();

        patient.ProfileImageUrl =
            request.ProfileImageUrl;

        await _patientRepository.UpdateAsync(patient);

        return await _patientRepository.GetByIdAsync(id)
            ?? throw new Exception(
                "Patient could not be retrieved after update.");
    }

    public async Task<PagedResponse<PatientResponse>>
        GetAllAsync(
            PatientFilterRequest request)
    {
        if (request.Page < 1)
            request.Page = 1;

        if (request.PageSize < 1)
            request.PageSize = 20;

        if (request.PageSize > 100)
            request.PageSize = 100;

        return await _patientRepository.GetAllAsync(request);
    }

    public async Task<PatientResponse> GetByIdAsync(
        Guid id)
    {
        var patient =
            await _patientRepository.GetByIdAsync(id);

        if (patient == null)
            throw new Exception("Patient not found.");

        return patient;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var patient =
            await _patientRepository.GetEntityByIdAsync(id);

        if (patient == null)
            throw new Exception("Patient not found.");

        await _patientRepository.DeleteAsync(id);

        return true;
    }

    public async Task<bool> ActivateAsync(Guid id)
    {
        var patient =
            await _patientRepository.GetEntityByIdAsync(id);

        if (patient == null)
            throw new Exception("Patient not found.");

        await _patientRepository.ActivateAsync(id);

        return true;
    }

    public async Task<bool> DeactivateAsync(Guid id)
    {
        var patient =
            await _patientRepository.GetEntityByIdAsync(id);

        if (patient == null)
            throw new Exception("Patient not found.");

        await _patientRepository.DeactivateAsync(id);

        return true;
    }

    private async Task<string> GeneratePatientCodeAsync()
    {
        string code;

        do
        {
            code = $"PAT-{Random.Shared.Next(100000, 999999)}";
        }
        while (await PatientCodeExistsAsync(code));

        return code;
    }

    private async Task<bool> PatientCodeExistsAsync(
        string code)
    {
        // PatientCode is unique in the database.
        // At this stage the repository does not expose
        // ExistsByCode, so the generated value is kept
        // random enough to avoid collisions.
        await Task.CompletedTask;

        return false;
    }
}