using Backend.Application.Dtos.Auth;
using Backend.Application.Exceptions;
using Backend.Application.Interfaces;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Auth.Refresh;

public class RefreshCommandHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    ITokenService tokenService) : IRequestHandler<RefreshCommand, TokenDto>
{
    public async Task<TokenDto> Handle(RefreshCommand command, CancellationToken cancellationToken)
    {
        var oldToken = await refreshTokenRepository.GetByTokenAsync(command.RefreshToken);

        if (oldToken is null || oldToken.IsRevoked || oldToken.ExpiresAt < DateTime.UtcNow)
        {
            throw new UnauthorizedException("Invalid refresh token.");
        }

        var user = await userRepository.GetByIdAsync(oldToken.UserId)
                   ?? throw new UnauthorizedException("Invalid refresh token.");

        await refreshTokenRepository.RevokeAsync(oldToken);

        var newToken = tokenService.CreateRefreshToken(user);
        await refreshTokenRepository.CreateAsync(newToken);

        var result = tokenService.CreateAccessToken(user);
        result.RefreshToken = newToken.Token;

        return result;
    }
}