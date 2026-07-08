namespace CoreAuth.Domain.Entities;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = null!;
    public DateTime ExpiresDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? RevokedDate { get; set; }
    public string? ReplacedByToken { get; set; }
    public string UserId { get; set; } = null!;
    public AppUser User { get; set; } = null!;
    public bool IsExpired => DateTime.UtcNow >= ExpiresDate;
    public bool IsRevoked => RevokedDate != null;
    public bool IsActive => !IsExpired && !IsRevoked;
}