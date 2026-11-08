using MediatR;

namespace Backend.Application.UseCases.Users.DeleteUser;

public class DeleteUserCommand : IRequest
{
    public required Guid Id { get; set; }

    public required Guid CurrentUserId { get; set; }
}