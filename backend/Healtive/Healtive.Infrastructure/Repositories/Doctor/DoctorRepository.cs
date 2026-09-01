using Dapper;
using Healtive.Application.DTOs.Common;
using Healtive.Application.DTOs.Doctor;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.Doctors;

public class DoctorRepository : IDoctorRepository
{
    private readonly IDbConnectionFactory _db;

    public DoctorRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<bool> ExistsByCodeAsync(
        Guid hospitalId,
        string doctorCode)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Doctors
WHERE HospitalId = @HospitalId
AND DoctorCode = @DoctorCode
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                HospitalId = hospitalId,
                DoctorCode = doctorCode.Trim()
            }) > 0;
    }

    public async Task<bool> ExistsByRegistrationNumberAsync(
        string registrationNumber)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Doctors
WHERE RegistrationNumber = @RegistrationNumber
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                RegistrationNumber =
                    registrationNumber.Trim()
            }) > 0;
    }

    public async Task<bool> ExistsByRegistrationNumberAsync(
        Guid doctorId,
        string registrationNumber)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Doctors
WHERE RegistrationNumber = @RegistrationNumber
AND Id <> @DoctorId
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                DoctorId = doctorId,
                RegistrationNumber =
                    registrationNumber.Trim()
            }) > 0;
    }

    public async Task<bool> ExistsByEmailAsync(
        string email)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Users
WHERE Email = @Email
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                Email = email.Trim()
            }) > 0;
    }

    public async Task<bool> ExistsByMobileAsync(
        string mobileNumber)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Users
WHERE MobileNumber = @MobileNumber
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                MobileNumber = mobileNumber.Trim()
            }) > 0;
    }

    public async Task CreateAsync(
     Doctor doctor,
     User user,
     Role role,
     UserRole userRole)
    {
        using var connection = _db.CreateConnection();

        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            // =====================================================
            // 1. CREATE USER
            // =====================================================

            const string userSql = @"
INSERT INTO Users
(
    Id,
    HospitalId,
    EmployeeCode,
    Username,
    FirstName,
    LastName,
    Email,
    MobileNumber,
    PasswordHash,
    IsEmailVerified,
    IsMobileVerified,
    IsActive,
    CreatedAt,
    IsDeleted
)
VALUES
(
    @Id,
    @HospitalId,
    @EmployeeCode,
    @Username,
    @FirstName,
    @LastName,
    @Email,
    @MobileNumber,
    @PasswordHash,
    @IsEmailVerified,
    @IsMobileVerified,
    @IsActive,
    @CreatedAt,
    @IsDeleted
);";

            await connection.ExecuteAsync(
                userSql,
                user,
                transaction);


            // =====================================================
            // 2. CREATE DOCTOR
            // =====================================================

            const string doctorSql = @"
INSERT INTO Doctors
(
    Id,
    HospitalId,
    UserId,
    FullName,
    DoctorCode,
    RegistrationNumber,
    Qualification,
    ExperienceYears,
    ConsultationFee,
    Gender,
    DateOfBirth,
    JoiningDate,
    Bio,
    ProfileImageUrl,
    IsAvailable,
    IsActive,
    CreatedAt,
    IsDeleted
)
VALUES
(
    @Id,
    @HospitalId,
    @UserId,
    @FullName,
    @DoctorCode,
    @RegistrationNumber,
    @Qualification,
    @ExperienceYears,
    @ConsultationFee,
    @Gender,
    @DateOfBirth,
    @JoiningDate,
    @Bio,
    @ProfileImageUrl,
    @IsAvailable,
    @IsActive,
    @CreatedAt,
    @IsDeleted
);";

            await connection.ExecuteAsync(
                doctorSql,
                doctor,
                transaction);


            // =====================================================
            // 3. ASSIGN DOCTOR ROLE TO USER
            // =====================================================

            const string userRoleSql = @"
INSERT INTO UserRoles
(
    UserId,
    RoleId,
    AssignedAt
)
VALUES
(
    @UserId,
    @RoleId,
    @AssignedAt
);";

            await connection.ExecuteAsync(
                userRoleSql,
                userRole,
                transaction);


            // =====================================================
            // 4. COMMIT
            // =====================================================

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task UpdateAsync(
        Doctor doctor,
        User user)
    {
        using var connection = _db.CreateConnection();

        connection.Open();

        using var transaction =
             connection.BeginTransaction();

        try
        {
            const string doctorSql = @"
UPDATE Doctors
SET
    FullName = @FullName,
    RegistrationNumber = @RegistrationNumber,
    Qualification = @Qualification,
    ExperienceYears = @ExperienceYears,
    ConsultationFee = @ConsultationFee,
    Gender = @Gender,
    DateOfBirth = @DateOfBirth,
    JoiningDate = @JoiningDate,
    Bio = @Bio,
    ProfileImageUrl = @ProfileImageUrl,
    UpdatedAt = @UpdatedAt
WHERE Id = @Id
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

            await connection.ExecuteAsync(
                doctorSql,
                doctor,
                transaction);

            const string userSql = @"
UPDATE Users
SET
    Email = @Email,
    MobileNumber = @MobileNumber
WHERE Id = @Id
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

            await connection.ExecuteAsync(
                userSql,
                user,
                transaction);

            transaction.Commit();
        }
        catch
        {
             transaction.Rollback();
            throw;
        }
    }

    public async Task<PagedResponse<DoctorListResponse>> GetAllAsync(
        Guid hospitalId,
        DoctorFilterRequest request)
    {
        using var connection = _db.CreateConnection();

        var conditions = @"
WHERE d.HospitalId = @HospitalId
AND d.IsDeleted = 0";

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            conditions += @"
AND
(
    d.FullName LIKE @Search
    OR d.DoctorCode LIKE @Search
    OR d.RegistrationNumber LIKE @Search
    OR d.Qualification LIKE @Search
)";
        }

        if (request.Status.HasValue)
        {
            conditions += @"
AND d.IsActive = @Status";
        }

        if (request.Available.HasValue)
        {
            conditions += @"
AND d.IsAvailable = @Available";
        }

        const string countSql = @"
SELECT COUNT(*)
FROM Doctors d
WHERE d.HospitalId = @HospitalId
AND d.IsDeleted = 0
";

        var countConditions = "";

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            countConditions += @"
AND
(
    d.FullName LIKE @Search
    OR d.DoctorCode LIKE @Search
    OR d.RegistrationNumber LIKE @Search
    OR d.Qualification LIKE @Search
)";
        }

        if (request.Status.HasValue)
        {
            countConditions += @"
AND d.IsActive = @Status";
        }

        if (request.Available.HasValue)
        {
            countConditions += @"
AND d.IsAvailable = @Available";
        }

        var parameters = new
        {
            HospitalId = hospitalId,
            Search = $"%{request.Search}%",
            request.Status,
            request.Available
        };

        var totalCount =
            await connection.ExecuteScalarAsync<int>(
                countSql + countConditions,
                parameters);

        var offset =
            (request.Page - 1) * request.PageSize;

        var sql = $@"
SELECT
    d.Id AS DoctorId,
    d.UserId,
    d.FullName,
    d.DoctorCode,
    d.RegistrationNumber,
    d.Qualification,
    d.ExperienceYears,
    d.ConsultationFee,
    d.Gender,
    d.IsAvailable,
    d.IsActive,
    d.CreatedAt
FROM Doctors d
{conditions}
ORDER BY d.CreatedAt DESC
LIMIT @PageSize OFFSET @Offset;";

        var items =
            await connection.QueryAsync<DoctorListResponse>(
                sql,
                new
                {
                    HospitalId = hospitalId,
                    Search = $"%{request.Search}%",
                    request.Status,
                    request.Available,
                    request.PageSize,
                    Offset = offset
                });

        var totalPages =
            request.PageSize == 0
                ? 0
                : (int)Math.Ceiling(
                    totalCount /
                    (double)request.PageSize);

        return new PagedResponse<DoctorListResponse>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }

    public async Task<DoctorResponse?> GetByIdAsync(
        Guid hospitalId,
        Guid doctorId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    Id AS DoctorId,
    HospitalId,
    UserId,
    FullName,
    DoctorCode,
    RegistrationNumber,
    Qualification,
    ExperienceYears,
    ConsultationFee,
    Gender,
    DateOfBirth,
    JoiningDate,
    Bio,
    ProfileImageUrl,
    IsAvailable,
    IsActive,
    CreatedAt,
    UpdatedAt
FROM Doctors
WHERE Id = @DoctorId
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

        return await connection.QueryFirstOrDefaultAsync<DoctorResponse>(
            sql,
            new
            {
                DoctorId = doctorId,
                HospitalId = hospitalId
            });
    }

    public async Task<Doctor?> GetEntityByIdAsync(
        Guid hospitalId,
        Guid doctorId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT *
FROM Doctors
WHERE Id = @DoctorId
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

        return await connection.QueryFirstOrDefaultAsync<Doctor>(
            sql,
            new
            {
                DoctorId = doctorId,
                HospitalId = hospitalId
            });
    }

    public async Task<Doctor?> GetByUserIdAsync(
    Guid hospitalId,
    Guid userId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    Id,
    HospitalId,
    UserId,
    FullName,
    DoctorCode,
    RegistrationNumber,
    Qualification,
    ExperienceYears,
    ConsultationFee,
    Gender,
    DateOfBirth,
    JoiningDate,
    Bio,
    ProfileImageUrl,
    IsAvailable,
    IsActive,
    CreatedAt,
    UpdatedAt,
    IsDeleted
FROM Doctors
WHERE UserId = @UserId
AND HospitalId = @HospitalId
AND IsDeleted = 0
AND IsActive = 1
LIMIT 1;";

        return await connection.QueryFirstOrDefaultAsync<Doctor>(
            sql,
            new
            {
                UserId = userId,
                HospitalId = hospitalId
            });
    }

    public async Task<User?> GetUserByIdAsync(
        Guid userId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT *
FROM Users
WHERE Id = @UserId
AND IsDeleted = 0;";

        return await connection.QueryFirstOrDefaultAsync<User>(
            sql,
            new
            {
                UserId = userId
            });
    }

    public async Task DeleteAsync(
        Guid hospitalId,
        Guid doctorId)
    {
        using var connection = _db.CreateConnection();

        connection.Open();

        using var transaction =
            connection.BeginTransaction();

        try
        {
            const string doctorSql = @"
UPDATE Doctors
SET
    IsDeleted = 1,
    IsActive = 0,
    IsAvailable = 0,
    UpdatedAt = @UpdatedAt
WHERE Id = @DoctorId
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

            await connection.ExecuteAsync(
                doctorSql,
                new
                {
                    DoctorId = doctorId,
                    HospitalId = hospitalId,
                    UpdatedAt = DateTime.UtcNow
                },
                transaction);

            const string userSql = @"
UPDATE Users u
INNER JOIN Doctors d
    ON d.UserId = u.Id
SET
    u.IsDeleted = 1,
    u.IsActive = 0
WHERE d.Id = @DoctorId
AND d.HospitalId = @HospitalId;";

            await connection.ExecuteAsync(
                userSql,
                new
                {
                    DoctorId = doctorId,
                    HospitalId = hospitalId
                },
                transaction);

            transaction.Commit();
        }
        catch
        {
             transaction.Rollback();
            throw;
        }
    }

    public async Task ActivateAsync(
        Guid hospitalId,
        Guid doctorId)
    {
        using var connection = _db.CreateConnection();

        connection.Open();

        using var transaction =
             connection.BeginTransaction();

        try
        {
            const string doctorSql = @"
UPDATE Doctors
SET
    IsActive = 1,
    IsAvailable = 1,
    UpdatedAt = @UpdatedAt
WHERE Id = @DoctorId
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

            await connection.ExecuteAsync(
                doctorSql,
                new
                {
                    DoctorId = doctorId,
                    HospitalId = hospitalId,
                    UpdatedAt = DateTime.UtcNow
                },
                transaction);

            const string userSql = @"
UPDATE Users u
INNER JOIN Doctors d
    ON d.UserId = u.Id
SET
    u.IsActive = 1
WHERE d.Id = @DoctorId
AND d.HospitalId = @HospitalId
AND u.IsDeleted = 0;";

            await connection.ExecuteAsync(
                userSql,
                new
                {
                    DoctorId = doctorId,
                    HospitalId = hospitalId
                },
                transaction);

            transaction.Commit();
        }
        catch
        {
             transaction.Rollback();
            throw;
        }
    }

    public async Task DeactivateAsync(
        Guid hospitalId,
        Guid doctorId)
    {
        using var connection = _db.CreateConnection();

        connection.Open();

        using var transaction =
             connection.BeginTransaction();

        try
        {
            const string doctorSql = @"
UPDATE Doctors
SET
    IsActive = 0,
    IsAvailable = 0,
    UpdatedAt = @UpdatedAt
WHERE Id = @DoctorId
AND HospitalId = @HospitalId
AND IsDeleted = 0;";

            await connection.ExecuteAsync(
                doctorSql,
                new
                {
                    DoctorId = doctorId,
                    HospitalId = hospitalId,
                    UpdatedAt = DateTime.UtcNow
                },
                transaction);

            const string userSql = @"
UPDATE Users u
INNER JOIN Doctors d
    ON d.UserId = u.Id
SET
    u.IsActive = 0
WHERE d.Id = @DoctorId
AND d.HospitalId = @HospitalId
AND u.IsDeleted = 0;";

            await connection.ExecuteAsync(
                userSql,
                new
                {
                    DoctorId = doctorId,
                    HospitalId = hospitalId
                },
                transaction);

             transaction.Commit();
        }
        catch
        {
             transaction.Rollback();
            throw;
        }
    }
    public async Task<Role?> GetRoleByNameAsync(
    Guid hospitalId,
    string roleName)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    Id,
    HospitalId,
    Name,
    Description,
    IsSystemRole,
    IsActive,
    CreatedAt,
    IsDeleted
FROM Roles
WHERE HospitalId = @HospitalId
AND Name = @Name
AND IsDeleted = 0
LIMIT 1;";

        return await connection.QueryFirstOrDefaultAsync<Role>(
            sql,
            new
            {
                HospitalId = hospitalId,
                Name = roleName
            });
    }
    public async Task ResetPasswordAsync(
    Guid hospitalId,
    Guid doctorId,
    string passwordHash)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Users u
INNER JOIN Doctors d
    ON d.UserId = u.Id
SET
    u.PasswordHash = @PasswordHash
WHERE d.Id = @DoctorId
AND d.HospitalId = @HospitalId
AND d.IsDeleted = 0
AND u.IsDeleted = 0;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                DoctorId = doctorId,
                HospitalId = hospitalId,
                PasswordHash = passwordHash
            });
    }

}