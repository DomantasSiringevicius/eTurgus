using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Auth.Logout;

public class LogoutCommandHandler(IRefreshTokenRepository refreshTokenRepository) : IRequestHandler<LogoutCommand>
{
    public async Task Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        var refreshToken = await refreshTokenRepository.GetByTokenAsync(command.RefreshToken);

        if (refreshToken is not null && !refreshToken.IsRevoked)
        {
            await refreshTokenRepository.RevokeAsync(refreshToken);
        }
    }
}