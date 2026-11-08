using Backend.Application.Dtos.Auth;
using Backend.Domain.Entities;

namespace Backend.Application.Interfaces;

public interface ITokenService
{
    TokenDto CreateAccessToken(User user);
    
    RefreshToken CreateRefreshToken(User user);
}