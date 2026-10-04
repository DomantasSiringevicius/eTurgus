using Backend.Application.Dtos.Shop;
using Backend.Application.UseCases.Products.GetAllProducts;
using Backend.Application.UseCases.Shops.CreateShop;
using Backend.Application.UseCases.Shops.DeleteShop;
using Backend.Application.UseCases.Shops.GetAllShops;
using Backend.Application.UseCases.Shops.GetShop;
using Backend.Application.UseCases.Shops.UpdateShop;
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

    [HttpPut("{name}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] string name, [FromForm] UpdateShopDto dto)
    {
        try
        {
            var result = await mediator.Send(new UpdateShopCommand() { Name = name, ShopDto = dto });
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpDelete("{name}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string name)
    {
        try
        {
            var result = await mediator.Send(new DeleteShopCommand() { Name = name });
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    [HttpGet("{name}")]
    public async Task<IActionResult> GetByNameAsync([FromRoute] string name)
    {
        try
        {
            var result = await mediator.Send(new GetShopQuery(name));
            return Ok(result);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var result = await mediator.Send(new GetAllShopsQuery());
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}