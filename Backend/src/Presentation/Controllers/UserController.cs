using Backend.Application.Dtos.Auth;
using Backend.Application.Exceptions;
using Backend.Application.UseCases.Users.ChangeUserRole;
using Backend.Application.UseCases.Users.DeleteUser;
using Backend.Application.UseCases.Users.GetAllUsers;
using Backend.Domain.Entities;
using Backend.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Presentation.Controllers;

[ApiController]
[Route("api/users")]
[Authorize(Roles = Roles.Admin)]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? role = null)
    {
        if (page < 1 || pageSize < 1 || pageSize > 50)
        {
            return BadRequest("page must be >= 1 and pageSize must be between 1 and 50.");
        }

        if (role != null && role != Roles.Admin && role != Roles.Seller && role != Roles.Buyer)
        {
            return BadRequest("role must be 'Admin', 'Seller' or 'Buyer'.");
        }

        var result = await mediator.Send(new GetAllUsersQuery { Page = page, PageSize = pageSize, Role = role });
        return Ok(result);
    }

    [HttpPut("{userId:guid}/role")]
    public async Task<IActionResult> ChangeRoleAsync([FromRoute] Guid userId, [FromForm] ChangeUserRoleDto dto)
    {
        try
        {
            var result = await mediator.Send(new ChangeUserRoleCommand
            {
                Id = userId,
                Role = dto.Role,
                CurrentUserId = User.GetUserId()
            });
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ValidationException ex)
        {
            return UnprocessableEntity(ex.Message);
        }
    }

    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid userId)
    {
        try
        {
            await mediator.Send(new DeleteUserCommand
            {
                Id = userId,
                CurrentUserId = User.GetUserId()
            });
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ValidationException ex)
        {
            return UnprocessableEntity(ex.Message);
        }
    }
}