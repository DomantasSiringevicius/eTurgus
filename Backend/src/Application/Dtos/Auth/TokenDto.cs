using System.Text.Json.Serialization;

namespace Backend.Application.Dtos.Auth;

public class TokenDto
{
    public required string AccessToken { get; set; }
    
    public required DateTime ExpiresAt { get; set; }

    [JsonIgnore]
    public string RefreshToken { get; set; } = string.Empty;

    [JsonIgnore]
    public DateTime RefreshTokenExpiresAt { get; set; }
    
    public string TokenType { get; set; } = "Bearer";
}