using Backend.Application.Dtos.Auth;
using Backend.Application.Exceptions;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Auth.GetCurrentUser;

public class GetCurrentUserQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetCurrentUserQuery, UserDto>
{
    public async Task<UserDto> Handle(GetCurrentUserQuery query, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(query.Id)
                   ?? throw new NotFoundException($"User with id '{query.Id}' not found.");

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            Role = user.Role
        };
    }
}