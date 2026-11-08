using System.Security.Claims;
using Backend.Application.Exceptions;
using Backend.Application.UseCases.Auth.GetCurrentUser;
using Backend.Application.UseCases.Auth.Login;
using Backend.Application.UseCases.Auth.Register;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Presentation.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromForm] RegisterCommand command)
    {
        try
        {
            var result = await mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, result);
        }
        catch (ConflictException ex)
        {
            return Conflict(ex.Message);
        }
        catch (ValidationException ex)
        {
            return UnprocessableEntity(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromForm] LoginCommand command)
    {
        try
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }
        catch (UnauthorizedException ex)
        {
            return Unauthorized(ex.Message);
        }
    }
    
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUserAsync()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        try
        {
            var result = await mediator.Send(new GetCurrentUserQuery(userId));
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}