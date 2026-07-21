namespace Healtive.Core.Entities;

public class UserRefreshToken
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string RefreshToken { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public bool IsRevoked { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? RevokedAt { get; set; }
}