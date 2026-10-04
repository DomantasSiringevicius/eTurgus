using Backend.Application.UseCases.Shops.CreateShop;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Presentation.Controllers;

[Route("api/[controller]")]
public class ShopController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromForm] CreateShopCommand command)
    {
        try
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}