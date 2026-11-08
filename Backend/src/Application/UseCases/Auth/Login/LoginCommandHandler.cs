using Backend.Application.Dtos.Auth;
using Backend.Application.Exceptions;
using Backend.Application.Interfaces;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Auth.Login;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService) : IRequestHandler<LoginCommand, TokenDto>
{
    public async Task<TokenDto> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(command.Email.Trim().ToLower());

        if (user is null || !passwordHasher.Verify(user, command.Password))
        {
            throw new UnauthorizedException("Invalid email or password.");
        }

        return tokenService.CreateAccessToken(user);
    }
}