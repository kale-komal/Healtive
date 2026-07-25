namespace Healtive.Application.DTOs.Dashboard;

public class DashboardResponse
{
    public int TotalHospitals { get; set; }

    public int ActiveHospitals { get; set; }

    public int InactiveHospitals { get; set; }

    public int TotalSubscriptionPlans { get; set; }

    public int ActiveSubscriptions { get; set; }

    public int ExpiredSubscriptions { get; set; }

    public int TrialSubscriptions { get; set; }

    public decimal MonthlyRevenue { get; set; }

    public decimal YearlyRevenue { get; set; }

    public int ExpiringIn7Days { get; set; }

    public int NewHospitalsThisMonth { get; set; }
}