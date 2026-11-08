using Backend.Application.Dtos.Auth;
using MediatR;

namespace Backend.Application.UseCases.Auth.Login;

public class LoginCommand : IRequest<TokenDto>
{
    public required string Email { get; set; }
    
    public required string Password { get; set; }
}