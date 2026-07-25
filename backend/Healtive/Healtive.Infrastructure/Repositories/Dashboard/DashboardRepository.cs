using Dapper;
using Healtive.Application.DTOs.Dashboard;
using Healtive.Application.Interfaces;
using Healtive.Infrastructure.Data;

namespace Healtive.Infrastructure.Repositories.Dashboard;

public class DashboardRepository : IDashboardRepository
{
    private readonly IDbConnectionFactory _db;

    public DashboardRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<DashboardResponse> GetDashboardAsync()
    {
        using var connection = _db.CreateConnection();

        var response = new DashboardResponse();

        // Total Hospitals
        response.TotalHospitals =
            await connection.ExecuteScalarAsync<int>(
                @"SELECT COUNT(*)
                  FROM Hospitals
                  WHERE IsDeleted = 0;");

        // Active Hospitals
        response.ActiveHospitals =
            await connection.ExecuteScalarAsync<int>(
                @"SELECT COUNT(*)
                  FROM Hospitals
                  WHERE IsActive = 1
                  AND IsDeleted = 0;");

        // Inactive Hospitals
        response.InactiveHospitals =
            await connection.ExecuteScalarAsync<int>(
                @"SELECT COUNT(*)
                  FROM Hospitals
                  WHERE IsActive = 0
                  AND IsDeleted = 0;");

        // Total Subscription Plans
        response.TotalSubscriptionPlans =
            await connection.ExecuteScalarAsync<int>(
                @"SELECT COUNT(*)
                  FROM SubscriptionPlans
                  WHERE IsActive = 1;");

        // Active Subscriptions
        response.ActiveSubscriptions =
            await connection.ExecuteScalarAsync<int>(
                @"SELECT COUNT(*)
                  FROM HospitalSubscriptions
                  WHERE IsActive = 1;");

        // Expired Subscriptions
        response.ExpiredSubscriptions =
            await connection.ExecuteScalarAsync<int>(
                @"SELECT COUNT(*)
                  FROM HospitalSubscriptions
                  WHERE EndDate < UTC_TIMESTAMP();");

        // Trial Subscriptions
        response.TrialSubscriptions =
            await connection.ExecuteScalarAsync<int>(
                @"SELECT COUNT(*)
                  FROM HospitalSubscriptions
                  WHERE TrialEndsOn IS NOT NULL
                  AND TrialEndsOn >= UTC_TIMESTAMP();");

        // Monthly Revenue
        response.MonthlyRevenue =
            await connection.ExecuteScalarAsync<decimal?>(
                @"SELECT SUM(AmountPaid)
                  FROM HospitalSubscriptions
                  WHERE YEAR(StartDate)=YEAR(CURDATE())
                  AND MONTH(StartDate)=MONTH(CURDATE());")
            ?? 0;

        // Yearly Revenue
        response.YearlyRevenue =
            await connection.ExecuteScalarAsync<decimal?>(
                @"SELECT SUM(AmountPaid)
                  FROM HospitalSubscriptions
                  WHERE YEAR(StartDate)=YEAR(CURDATE());")
            ?? 0;

        // Expiring In Next 7 Days
        response.ExpiringIn7Days =
            await connection.ExecuteScalarAsync<int>(
                @"SELECT COUNT(*)
                  FROM HospitalSubscriptions
                  WHERE EndDate BETWEEN CURDATE()
                  AND DATE_ADD(CURDATE(), INTERVAL 7 DAY);");

        // New Hospitals This Month
        response.NewHospitalsThisMonth =
            await connection.ExecuteScalarAsync<int>(
                @"SELECT COUNT(*)
                  FROM Hospitals
                  WHERE YEAR(CreatedAt)=YEAR(CURDATE())
                  AND MONTH(CreatedAt)=MONTH(CURDATE())
                  AND IsDeleted = 0;");

        return response;
    }
}