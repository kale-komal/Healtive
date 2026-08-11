using Dapper;
using Healtive.Application.DTOs.Subscription;
using Healtive.Application.Interfaces;
using Healtive.Core.Entities;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.HospitalSubscriptions;

public class HospitalSubscriptionRepository : IHospitalSubscriptionRepository
{
    private readonly IDbConnectionFactory _db;

    public HospitalSubscriptionRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<IEnumerable<HospitalSubscriptionListResponse>> GetAllAsync()
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT
    hs.Id,
    h.Name AS HospitalName,
    sp.Name AS PlanName,
    hs.StartDate,
    hs.EndDate,
    hs.AmountPaid,
    hs.PaymentStatus,
    hs.IsActive
FROM HospitalSubscriptions hs
INNER JOIN Hospitals h
    ON hs.HospitalId = h.Id
INNER JOIN SubscriptionPlans sp
    ON hs.SubscriptionPlanId = sp.Id
ORDER BY hs.CreatedAt DESC;";

        return await connection.QueryAsync<HospitalSubscriptionListResponse>(sql);
    }

    public async Task<HospitalSubscription?> GetByIdAsync(Guid id)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
SELECT *
FROM HospitalSubscriptions
WHERE Id=@Id;";

        return await connection.QueryFirstOrDefaultAsync<HospitalSubscription>(
            sql,
            new
            {
                Id = id
            });
    }

    public async Task CreateAsync(HospitalSubscription subscription)
    {
        using var connection = _db.CreateConnection();


        const string sql = @"
INSERT INTO HospitalSubscriptions
(
    Id,
    HospitalId,
    SubscriptionPlanId,
    StartDate,
    EndDate,
    TrialEndsOn,
    AmountPaid,
    PaymentStatus,
    IsActive,
    CreatedAt
)
VALUES
(
    @Id,
    @HospitalId,
    @SubscriptionPlanId,
    @StartDate,
    @EndDate,
    @TrialEndsOn,
    @AmountPaid,
    @PaymentStatus,
    @IsActive,
    @CreatedAt
);";

        await connection.ExecuteAsync(sql, subscription);
    }

    public async Task UpdateAsync(HospitalSubscription subscription)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
UPDATE HospitalSubscriptions
SET
    HospitalId=@HospitalId,
    SubscriptionPlanId=@SubscriptionPlanId,
    StartDate=@StartDate,
    EndDate=@EndDate,
    TrialEndsOn=@TrialEndsOn,
    AmountPaid=@AmountPaid,
    PaymentStatus=@PaymentStatus,
    IsActive=@IsActive,
    UpdatedAt=@UpdatedAt
WHERE Id=@Id;";

        await connection.ExecuteAsync(sql, subscription);
    }

    public async Task DeleteAsync(Guid id)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
DELETE FROM HospitalSubscriptions
WHERE Id=@Id;";

        await connection.ExecuteAsync(
            sql,
            new
            {
                Id = id
            });
    }
    public async Task<bool> HasActiveSubscriptionAsync(Guid hospitalId)
    {
        using var connection = _db.CreateConnection();

        const string sql = @"
        SELECT COUNT(1)
        FROM HospitalSubscriptions
        WHERE HospitalId = @HospitalId
        AND IsActive = 1;
    ";

        var count = await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                HospitalId = hospitalId
            });

        return count > 0;
    }
}