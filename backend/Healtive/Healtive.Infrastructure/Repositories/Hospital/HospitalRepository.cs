using Dapper;
using Healtive.Application.DTOs.Hospital;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;
using Healtive.Infrastructure.Data;
using Healtive.Application.DTOs.Common;


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
    public async Task<PagedResponse<HospitalListResponse>> GetAllAsync(
    HospitalFilterRequest request)
    {
        using var connection = _db.CreateConnection();

        var whereClause = "WHERE IsDeleted = 0";

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            whereClause += @" AND (
                            Name LIKE @Search
                            OR Code LIKE @Search
                            OR Email LIKE @Search
                         )";
        }

        if (request.IsActive.HasValue)
        {
            whereClause += " AND IsActive = @IsActive";
        }

        var parameters = new
        {
            Search = $"%{request.Search}%",
            request.IsActive
        };

        // Total Records
        var countSql = $@"
        SELECT COUNT(*)
        FROM Hospitals
        {whereClause};
    ";

        var totalRecords = await connection.ExecuteScalarAsync<int>(
            countSql,
            parameters);

        // Data
        var sql = $@"
SELECT
    Id AS HospitalId,
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
{whereClause}
ORDER BY CreatedAt DESC
LIMIT @PageSize OFFSET @Offset;
";

        var hospitals = await connection.QueryAsync<HospitalListResponse>(
            sql,
            new
            {
                Search = $"%{request.Search}%",
                request.IsActive,
                Offset = (request.Page - 1) * request.PageSize,
                PageSize = request.PageSize
            });

        return new PagedResponse<HospitalListResponse>
        {
            Items = hospitals.ToList(),
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalRecords,
            TotalPages = (int)Math.Ceiling((double)totalRecords / request.PageSize)
        };
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
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Hospitals
SET
    IsDeleted = 1,
    IsActive = 0,
    UpdatedAt = @UpdatedAt
WHERE Id = @Id
AND IsDeleted = 0;";

        await connection.ExecuteAsync(sql, new
        {
            Id = id,
            UpdatedAt = DateTime.UtcNow
        });
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

    public async Task ActivateAsync(Guid id)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Hospitals
SET
    IsActive = 1,
    UpdatedAt = @UpdatedAt
WHERE Id = @Id
AND IsDeleted = 0;";

        await connection.ExecuteAsync(sql, new
        {
            Id = id,
            UpdatedAt = DateTime.UtcNow
        });
    }

    public async Task DeactivateAsync(Guid id)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE Hospitals
SET
    IsActive = 0,
    UpdatedAt = @UpdatedAt
WHERE Id = @Id
AND IsDeleted = 0;";

        await connection.ExecuteAsync(sql, new
        {
            Id = id,
            UpdatedAt = DateTime.UtcNow
        });
    }

    public async Task<string?> GetLastHospitalCodeAsync()
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT Code
FROM Hospitals
ORDER BY CreatedAt DESC
LIMIT 1;";

        return await connection.QueryFirstOrDefaultAsync<string>(sql);
    }
}