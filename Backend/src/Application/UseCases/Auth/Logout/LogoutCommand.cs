using MediatR;

namespace Backend.Application.UseCases.Auth.Logout;

public class LogoutCommand : IRequest
{
    public required string RefreshToken { get; set; }
}