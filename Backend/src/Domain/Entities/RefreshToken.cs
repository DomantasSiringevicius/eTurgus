namespace Backend.Domain.Entities;

public class RefreshToken : Entity
{
    public required string Token { get; set; }
    
    public required Guid UserId { get; set; }
    
    public required DateTime ExpiresAt { get; set; }
    
    public bool IsRevoked { get; set; }
}