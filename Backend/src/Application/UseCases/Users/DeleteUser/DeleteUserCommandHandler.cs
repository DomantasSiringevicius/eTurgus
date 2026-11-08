using Backend.Application.Exceptions;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Users.DeleteUser;

public class DeleteUserCommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        if (command.Id == command.CurrentUserId)
        {
            throw new ValidationException("You cannot delete yourself.");
        }

        var user = await userRepository.GetByIdAsync(command.Id)
                   ?? throw new NotFoundException($"User with id '{command.Id}' not found.");

        await userRepository.DeleteAsync(user);
    }
}