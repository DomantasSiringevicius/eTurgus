using Backend.Application.Dtos.Auth;
using Backend.Application.Exceptions;
using Backend.Domain.Entities;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Users.ChangeUserRole;

public class ChangeUserRoleCommandHandler(IUserRepository userRepository)
    : IRequestHandler<ChangeUserRoleCommand, UserDto>
{
    public async Task<UserDto> Handle(ChangeUserRoleCommand command, CancellationToken cancellationToken)
    {
        if (command.Role != Roles.Admin && command.Role != Roles.Seller && command.Role != Roles.Buyer)
        {
            throw new ValidationException("Role must be 'Admin', 'Seller' or 'Buyer'.");
        }

        if (command.Id == command.CurrentUserId)
        {
            throw new ValidationException("You cannot change your own role.");
        }

        var user = await userRepository.GetByIdAsync(command.Id)
                   ?? throw new NotFoundException($"User with id '{command.Id}' not found.");

        user.Role = command.Role;
        await userRepository.UpdateAsync(user);

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            Role = user.Role
        };
    }
}