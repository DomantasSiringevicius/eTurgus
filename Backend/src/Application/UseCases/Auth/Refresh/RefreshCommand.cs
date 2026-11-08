using Backend.Application.Dtos.Auth;
using MediatR;

namespace Backend.Application.UseCases.Auth.Refresh;

public class RefreshCommand : IRequest<TokenDto>
{
    public required string RefreshToken { get; set; }
}