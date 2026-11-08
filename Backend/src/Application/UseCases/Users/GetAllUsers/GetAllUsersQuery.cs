using Backend.Application.Dtos;
using Backend.Application.Dtos.Auth;
using MediatR;

namespace Backend.Application.UseCases.Users.GetAllUsers;

public class GetAllUsersQuery : IRequest<PagedResult<UserDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Role { get; set; }
}