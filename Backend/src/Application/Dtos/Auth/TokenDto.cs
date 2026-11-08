namespace Backend.Application.Dtos.Auth;

public class TokenDto
{
    public required string AccessToken { get; set; }
    
    public required DateTime ExpiresAt { get; set; }
    
    public string TokenType { get; set; } = "Bearer";
}