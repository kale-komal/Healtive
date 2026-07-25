using Dapper;
using Healtive.Application.DTOs.SubscriptionPlan;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.SubscriptionPlans;

public class SubscriptionPlanRepository : ISubscriptionPlanRepository
{
    private readonly IDbConnectionFactory _db;

    public SubscriptionPlanRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task CreateAsync(SubscriptionPlan plan)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
INSERT INTO SubscriptionPlans
(
    Id,
    Name,
    Description,
    Price,
    DurationInDays,
    MaxBranches,
    MaxDoctors,
    MaxPatients,
    IsTrial,
    IsActive,
    CreatedAt
)
VALUES
(
    @Id,
    @Name,
    @Description,
    @Price,
    @DurationInDays,
    @MaxBranches,
    @MaxDoctors,
    @MaxPatients,
    @IsTrial,
    @IsActive,
    @CreatedAt
);";

        await connection.ExecuteAsync(sql, plan);
    }

    public async Task UpdateAsync(SubscriptionPlan plan)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE SubscriptionPlans
SET
    Name=@Name,
    Description=@Description,
    Price=@Price,
    DurationInDays=@DurationInDays,
    MaxBranches=@MaxBranches,
    MaxDoctors=@MaxDoctors,
    MaxPatients=@MaxPatients,
    IsTrial=@IsTrial,
    IsActive=@IsActive,
    UpdatedAt=@UpdatedAt
WHERE Id=@Id;";

        await connection.ExecuteAsync(sql, plan);
    }

    public async Task DeleteAsync(Guid id)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
DELETE FROM SubscriptionPlans
WHERE Id=@Id;";

        await connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<SubscriptionPlan?> GetByIdAsync(Guid id)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT *
FROM SubscriptionPlans
WHERE Id=@Id;";

        return await connection.QueryFirstOrDefaultAsync<SubscriptionPlan>(
            sql,
            new { Id = id });
    }

    public async Task<IEnumerable<SubscriptionPlanListResponse>> GetAllAsync()
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    Id,
    Name,
    Price,
    DurationInDays,
    MaxBranches,
    MaxDoctors,
    MaxPatients,
    IsTrial,
    IsActive
FROM SubscriptionPlans
ORDER BY Price;";

        return await connection.QueryAsync<SubscriptionPlanListResponse>(sql);
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM SubscriptionPlans
WHERE Name=@Name;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new { Name = name }) > 0;
    }

    public async Task<bool> ExistsByNameAsync(Guid id, string name)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT COUNT(*)
FROM SubscriptionPlans
WHERE Name=@Name
AND Id<>@Id;";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                Id = id,
                Name = name
            }) > 0;
    }
}