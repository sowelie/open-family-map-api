using System.ComponentModel.DataAnnotations;

namespace OpenFamilyMapAPI.Entities;

public class RefreshToken
{
    [Key] public string Token { get; set; }
    public User User { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRevoked { get; set; }
    public bool IsUsed { get; set; }
}