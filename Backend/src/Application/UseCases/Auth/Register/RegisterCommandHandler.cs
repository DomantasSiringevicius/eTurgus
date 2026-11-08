using Backend.Application.Dtos.Auth;
using Backend.Application.Exceptions;
using Backend.Application.Interfaces;
using Backend.Domain.Entities;
using Backend.Domain.IRepositories;
using MediatR;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace Backend.Application.UseCases.Auth.Register;

public class RegisterCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    : IRequestHandler<RegisterCommand, UserDto>
{
    public async Task<UserDto> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var email = command.Email.Trim().ToLower();

        if (!email.Contains('@'))
        {
            throw new ValidationException("Email is not valid.");
        }

        if (command.Password.Length < 6)
        {
            throw new ValidationException("Password must be at least 6 characters long.");
        }

        if (command.Role != Roles.Buyer && command.Role != Roles.Seller)
        {
            throw new ValidationException("Role must be either 'Buyer' or 'Seller'.");
        }

        var userInDb = await userRepository.GetByEmailAsync(email);

        if (userInDb is not null)
        {
            throw new ConflictException($"User with email '{email}' already exists.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            UserName = command.UserName,
            PasswordHash = string.Empty,
            Role = command.Role,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        user.PasswordHash = passwordHasher.Hash(user, command.Password);

        await userRepository.CreateAsync(user);

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            Role = user.Role
        };
    }
}