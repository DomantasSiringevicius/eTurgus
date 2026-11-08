using Backend.Application.Exceptions;
using Backend.Application.UseCases.Auth.Register;
using MediatR;
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
}