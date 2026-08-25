using Dapper;
using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Patient;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.Patients;

public class PatientRepository : IPatientRepository
{
    private readonly IDbConnectionFactory _db;

    public PatientRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<bool> ExistsByMobileAsync(
        string mobileNumber)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Patients
WHERE MobileNumber = @MobileNumber
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                MobileNumber = mobileNumber.Trim()
            }) > 0;
    }

    public async Task<bool> ExistsByMobileAsync(
        Guid id,
        string mobileNumber)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Patients
WHERE MobileNumber = @MobileNumber
AND Id <> @Id
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                Id = id,
                MobileNumber = mobileNumber.Trim()
            }) > 0;
    }

    public async Task<bool> ExistsByEmailAsync(
        string email)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Patients
WHERE LOWER(Email) = LOWER(@Email)
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                Email = email.Trim()
            }) > 0;
    }

    public async Task<bool> ExistsByEmailAsync(
        Guid id,
        string email)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Patients
WHERE LOWER(Email) = LOWER(@Email)
AND Id <> @Id
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                Id = id,
                Email = email.Trim()
            }) > 0;
    }

    public async Task CreateAsync(
        Patient patient)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
INSERT INTO Patients
(
    Id,
    PatientCode,
    FirstName,
    LastName,
    DateOfBirth,
    Gender,
    BloodGroup,
    MobileNumber,
    Email,
    PasswordHash,
    GoogleId,
    IsMobileVerified,
    IsEmailVerified,
    QRToken,
    ProfileImageUrl,
    LastLoginAt,
    IsActive,
    CreatedAt,
    IsDeleted
)
VALUES
(
    @Id,
    @PatientCode,
    @FirstName,
    @LastName,
    @DateOfBirth,
    @Gender,
    @BloodGroup,
    @MobileNumber,
    @Email,
    @PasswordHash,
    @GoogleId,
    @IsMobileVerified,
    @IsEmailVerified,
    @QRToken,
    @ProfileImageUrl,
    @LastLoginAt,
    @IsActive,
    @CreatedAt,
    @IsDeleted
);";

        await connection.ExecuteAsync(
            sql,
            new
            {
                patient.Id,
                patient.PatientCode,
                patient.FirstName,
                patient.LastName,

                DateOfBirth = patient.DateOfBirth?
                    .ToDateTime(TimeOnly.MinValue),

                patient.Gender,
                patient.BloodGroup,
                patient.MobileNumber,
                patient.Email,
                patient.PasswordHash,
                patient.GoogleId,
                patient.IsMobileVerified,
                patient.IsEmailVerified,
                patient.QRToken,
                patient.ProfileImageUrl,
                patient.LastLoginAt,
                patient.IsActive,
                patient.CreatedAt,
                patient.IsDeleted
            });
    }

    public async Task UpdateAsync(
        Patient patient)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Patients
SET
    FirstName = @FirstName,
    LastName = @LastName,
    DateOfBirth = @DateOfBirth,
    Gender = @Gender,
    BloodGroup = @BloodGroup,
    MobileNumber = @MobileNumber,
    Email = @Email,
    ProfileImageUrl = @ProfileImageUrl,
    UpdatedAt = @UpdatedAt
WHERE Id = @Id
AND IsDeleted = 0;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                patient.Id,
                patient.FirstName,
                patient.LastName,

                DateOfBirth = patient.DateOfBirth?
                    .ToDateTime(TimeOnly.MinValue),

                patient.Gender,
                patient.BloodGroup,
                patient.MobileNumber,
                patient.Email,
                patient.ProfileImageUrl,
                UpdatedAt = DateTime.UtcNow
            });
    }

    public async Task<PagedResponse<PatientResponse>>
        GetAllAsync(
            PatientFilterRequest request)
    {
        using var connection = _db.CreateConnection();

        var conditions = @"
WHERE IsDeleted = 0";

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            conditions += @"
AND
(
    PatientCode LIKE @Search
    OR FirstName LIKE @Search
    OR LastName LIKE @Search
    OR MobileNumber LIKE @Search
    OR Email LIKE @Search
)";
        }

        if (request.IsActive.HasValue)
        {
            conditions += @"
AND IsActive = @IsActive";
        }

        var countSql = $@"
SELECT COUNT(*)
FROM Patients
{conditions};";

        var totalCount =
            await connection.ExecuteScalarAsync<int>(
                countSql,
                new
                {
                    Search = $"%{request.Search}%",
                    request.IsActive
                });

        var offset =
            (request.Page - 1) * request.PageSize;

        var sql = $@"
SELECT
    Id AS PatientId,
    PatientCode,
    FirstName,
    LastName,
    DateOfBirth,
    Gender,
    BloodGroup,
    MobileNumber,
    Email,
    ProfileImageUrl,
    IsMobileVerified,
    IsEmailVerified,
    IsActive,
    CreatedAt
FROM Patients
{conditions}
ORDER BY CreatedAt DESC
LIMIT @PageSize OFFSET @Offset;";

        var rows =
            await connection.QueryAsync<PatientDbModel>(
                sql,
                new
                {
                    Search = $"%{request.Search}%",
                    request.IsActive,
                    request.PageSize,
                    Offset = offset
                });

        var items = rows.Select(MapToResponse);

        var totalPages =
            request.PageSize == 0
                ? 0
                : (int)Math.Ceiling(
                    totalCount /
                    (double)request.PageSize);

        return new PagedResponse<PatientResponse>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }

    public async Task<PatientResponse?>
        GetByIdAsync(Guid id)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    Id AS PatientId,
    PatientCode,
    FirstName,
    LastName,
    DateOfBirth,
    Gender,
    BloodGroup,
    MobileNumber,
    Email,
    ProfileImageUrl,
    IsMobileVerified,
    IsEmailVerified,
    IsActive,
    CreatedAt
FROM Patients
WHERE Id = @Id
AND IsDeleted = 0;";

        var row =
            await connection.QueryFirstOrDefaultAsync<PatientDbModel>(
                sql,
                new
                {
                    Id = id
                });

        return row == null
            ? null
            : MapToResponse(row);
    }

    public async Task<Patient?>
        GetEntityByIdAsync(Guid id)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    Id,
    PatientCode,
    FirstName,
    LastName,
    DateOfBirth,
    Gender,
    BloodGroup,
    MobileNumber,
    Email,
    PasswordHash,
    GoogleId,
    IsMobileVerified,
    IsEmailVerified,
    QRToken,
    ProfileImageUrl,
    LastLoginAt,
    IsActive,
    CreatedAt,
    UpdatedAt,
    IsDeleted
FROM Patients
WHERE Id = @Id
AND IsDeleted = 0;";

        var row =
            await connection.QueryFirstOrDefaultAsync<PatientDbModel>(
                sql,
                new
                {
                    Id = id
                });

        if (row == null)
            return null;

        return new Patient
        {
            Id = row.Id,
            PatientCode = row.PatientCode,
            FirstName = row.FirstName,
            LastName = row.LastName,
            DateOfBirth = row.DateOfBirth.HasValue
                ? DateOnly.FromDateTime(row.DateOfBirth.Value)
                : null,
            Gender = row.Gender,
            BloodGroup = row.BloodGroup,
            MobileNumber = row.MobileNumber,
            Email = row.Email,
            PasswordHash = row.PasswordHash,
            GoogleId = row.GoogleId,
            IsMobileVerified = row.IsMobileVerified,
            IsEmailVerified = row.IsEmailVerified,
            QRToken = row.QRToken,
            ProfileImageUrl = row.ProfileImageUrl,
            LastLoginAt = row.LastLoginAt,
            IsActive = row.IsActive,
            CreatedAt = row.CreatedAt,
            UpdatedAt = row.UpdatedAt,
            IsDeleted = row.IsDeleted
        };
    }

    public async Task DeleteAsync(Guid id)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Patients
SET
    IsDeleted = 1,
    IsActive = 0,
    UpdatedAt = @UpdatedAt
WHERE Id = @Id
AND IsDeleted = 0;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                Id = id,
                UpdatedAt = DateTime.UtcNow
            });
    }

    public async Task ActivateAsync(Guid id)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Patients
SET
    IsActive = 1,
    UpdatedAt = @UpdatedAt
WHERE Id = @Id
AND IsDeleted = 0;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                Id = id,
                UpdatedAt = DateTime.UtcNow
            });
    }

    public async Task DeactivateAsync(Guid id)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Patients
SET
    IsActive = 0,
    UpdatedAt = @UpdatedAt
WHERE Id = @Id
AND IsDeleted = 0;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                Id = id,
                UpdatedAt = DateTime.UtcNow
            });
    }

    private static PatientResponse MapToResponse(
        PatientDbModel row)
    {
        return new PatientResponse
        {
            PatientId = row.PatientId,
            PatientCode = row.PatientCode,
            FirstName = row.FirstName,
            LastName = row.LastName,
            DateOfBirth = row.DateOfBirth.HasValue
                ? DateOnly.FromDateTime(row.DateOfBirth.Value)
                : null,
            Gender = row.Gender,
            BloodGroup = row.BloodGroup,
            MobileNumber = row.MobileNumber,
            Email = row.Email,
            ProfileImageUrl = row.ProfileImageUrl,
            IsMobileVerified = row.IsMobileVerified,
            IsEmailVerified = row.IsEmailVerified,
            IsActive = row.IsActive,
            CreatedAt = row.CreatedAt
        };
    }

    private class PatientDbModel
    {
        public Guid Id { get; set; }

        public Guid PatientId { get; set; }

        public string PatientCode { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        public string Gender { get; set; } = string.Empty;

        public string? BloodGroup { get; set; }

        public string MobileNumber { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? PasswordHash { get; set; }

        public string? GoogleId { get; set; }

        public bool IsMobileVerified { get; set; }

        public bool IsEmailVerified { get; set; }

        public Guid QRToken { get; set; }

        public string? ProfileImageUrl { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }
    }
}