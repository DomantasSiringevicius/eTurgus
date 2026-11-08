using Backend.Application.Dtos.Auth;
using MediatR;

namespace Backend.Application.UseCases.Auth.GetCurrentUser;

public class GetCurrentUserQuery(Guid id) : IRequest<UserDto>
{
    public Guid Id { get; } = id;
}