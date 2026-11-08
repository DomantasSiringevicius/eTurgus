using Backend.Domain.Entities;

namespace Backend.Domain.IRepositories;

public interface IRefreshTokenRepository
{
    Task CreateAsync(RefreshToken refreshToken);

    Task<RefreshToken?> GetByTokenAsync(string token);

    Task RevokeAsync(RefreshToken refreshToken);
}