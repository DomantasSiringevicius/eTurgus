using Backend.Application.Dtos;
using Backend.Application.Dtos.Auth;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Users.GetAllUsers;

public class GetAllUsersQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetAllUsersQuery, PagedResult<UserDto>>
{
    public async Task<PagedResult<UserDto>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
    {
        var (users, totalCount) = await userRepository.GetPagedAsync(query.Page, query.PageSize, query.Role);

        return new PagedResult<UserDto>
        {
            Items = users.Select(user => new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Role = user.Role
            }).ToList(),
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }
}