using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Backend.Application.Dtos.Auth;
using Backend.Application.Interfaces;
using Backend.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Backend.Infrastructure.Security;

public class JwtTokenService(JwtSettings settings) : ITokenService
{
    public TokenDto CreateAccessToken(User user)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(settings.AccessTokenExpirationMinutes);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new Claim("role", user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key));
        var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: settings.Issuer,
            audience: settings.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credential
        );

        return new TokenDto
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expiresAt
        };
    }
    
    public RefreshToken CreateRefreshToken(User user)
    {
        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(settings.RefreshTokenExpirationDays),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}