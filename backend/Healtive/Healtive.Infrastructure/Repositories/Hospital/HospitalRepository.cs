using Dapper;
using Healtive.Application.DTOs.Hospital;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.Hospitals;

public class HospitalRepository : IHospitalRepository
{
    private readonly IDbConnectionFactory _db;

    public HospitalRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<bool> ExistsByCodeAsync(Guid id, string code)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Hospitals
WHERE Code=@Code
AND Id<>@Id
AND IsDeleted=0;";

        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            Id = id,
            Code = code
        }) > 0;
    }

    public async Task<bool> ExistsByEmailAsync(Guid id, string email)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Hospitals
WHERE Email=@Email
AND Id<>@Id
AND IsDeleted=0;";

        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            Id = id,
            Email = email
        }) > 0;
    }

    public async Task<bool> ExistsByMobileAsync(Guid id, string phoneNumber)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Hospitals
WHERE PhoneNumber=@PhoneNumber
AND Id<>@Id
AND IsDeleted=0;";

        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            Id = id,
            PhoneNumber = phoneNumber
        }) > 0;
    }
    public async Task<bool> ExistsByEmailAsync(string email)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Hospitals
WHERE Email=@Email
AND IsDeleted=0;";

        return await connection.ExecuteScalarAsync<int>(sql, new { Email = email }) > 0;
    }

    public async Task<bool> ExistsByMobileAsync(string phoneNumber)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Hospitals
WHERE PhoneNumber=@PhoneNumber
AND IsDeleted=0;";

        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            PhoneNumber = phoneNumber
        }) > 0;
    }
    

    public async Task CreateAsync(Hospital hospital)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
INSERT INTO Hospitals
(
    Id,
    Name,
    Code,
    LicenseNumber,
    GSTNumber,
    HospitalType,
    Email,
    PhoneNumber,
    Website,
    LogoUrl,
    Address,
    City,
    State,
    Country,
    PostalCode,
    TimeZone,
    Currency,
    IsActive,
    CreatedAt
)
VALUES
(
    @Id,
    @Name,
    @Code,
    @LicenseNumber,
    @GSTNumber,
    @HospitalType,
    @Email,
    @PhoneNumber,
    @Website,
    @LogoUrl,
    @Address,
    @City,
    @State,
    @Country,
    @PostalCode,
    @TimeZone,
    @Currency,
    @IsActive,
    @CreatedAt
);";

        await connection.ExecuteAsync(sql, hospital);
    }
    public async Task<IEnumerable<HospitalListResponse>> GetAllAsync()
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    Id            AS HospitalId,
    Code,
    Name,
    Email,
    PhoneNumber,
    HospitalType,
    City,
    State,
    IsActive,
    CreatedAt
FROM Hospitals
WHERE IsDeleted = 0
ORDER BY CreatedAt DESC;";

        return await connection.QueryAsync<HospitalListResponse>(sql);
    }

    public async Task<Hospital?> GetByIdAsync(Guid id)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    Id,
    Name,
    Code,
    LicenseNumber,
    GSTNumber,
    HospitalType,
    Email,
    PhoneNumber,
    Website,
    LogoUrl,
    Address,
    City,
    State,
    Country,
    PostalCode,
    TimeZone,
    Currency,
    IsActive,
    CreatedAt,
    UpdatedAt,
    IsDeleted
FROM Hospitals
WHERE Id = @Id
AND IsDeleted = 0;";

        return await connection.QueryFirstOrDefaultAsync<Hospital>(
            sql,
            new
            {
                Id = id
            });
    }

    public async Task UpdateAsync(Hospital hospital)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Hospitals
SET
    Name=@Name,
    Code=@Code,
    LicenseNumber=@LicenseNumber,
    GSTNumber=@GSTNumber,
    HospitalType=@HospitalType,
    Email=@Email,
    PhoneNumber=@PhoneNumber,
    Website=@Website,
    Address=@Address,
    City=@City,
    State=@State,
    Country=@Country,
    PostalCode=@PostalCode,
    TimeZone=@TimeZone,
    Currency=@Currency,
    UpdatedAt=@UpdatedAt
WHERE Id=@Id
AND IsDeleted=0;";

        await connection.ExecuteAsync(sql, hospital);
    }

    public async Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task CreateRoleAsync(Role role)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
INSERT INTO Roles
(
    Id,
    HospitalId,
    Name,
    Description,
    IsSystemRole,
    IsActive,
    CreatedAt,
    IsDeleted
)
VALUES
(
    @Id,
    @HospitalId,
    @Name,
    @Description,
    @IsSystemRole,
    @IsActive,
    @CreatedAt,
    @IsDeleted
);";

        await connection.ExecuteAsync(sql, role);
    }

    public async Task CreateUserAsync(User user)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
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

        await connection.ExecuteAsync(sql, user);
    }

    public async Task AssignRoleAsync(UserRole userRole)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
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

        await connection.ExecuteAsync(sql, userRole);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Users
WHERE Username=@Username
AND IsDeleted=0;";

        var count = await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                Username = username
            });

        return count > 0;
    }

    public async Task<bool> ExistsByCodeAsync(string code)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM Hospitals
WHERE Code = @Code
AND IsDeleted = 0;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                Code = code
            }) > 0;
    }
}