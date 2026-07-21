namespace Healtive.Core.Entities;

public class UserLoginHistory
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public DateTime LoginTime { get; set; }

    public DateTime? LogoutTime { get; set; }

    public string? IpAddress { get; set; }

    public string? Device { get; set; }

    public string? Browser { get; set; }

    public string? OperatingSystem { get; set; }

    public bool IsSuccessful { get; set; }
}