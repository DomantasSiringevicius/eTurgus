using Backend.Application.Dtos.Auth;
using MediatR;

namespace Backend.Application.UseCases.Users.ChangeUserRole;

public class ChangeUserRoleCommand : IRequest<UserDto>
{
    public required Guid Id { get; set; }

    public required string Role { get; set; }

    public required Guid CurrentUserId { get; set; }
}