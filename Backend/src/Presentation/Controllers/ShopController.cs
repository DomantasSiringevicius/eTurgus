using Backend.Application.Dtos.Shop;
using Backend.Application.Exceptions;
using Backend.Application.UseCases.Products.GetAllProducts;
using Backend.Application.UseCases.Shops.CreateShop;
using Backend.Application.UseCases.Shops.DeleteShop;
using Backend.Application.UseCases.Shops.GetAllShops;
using Backend.Application.UseCases.Shops.GetShop;
using Backend.Application.UseCases.Shops.GetShopSummary;
using Backend.Application.UseCases.Shops.UpdateShop;
using Backend.Presentation.Hypermedia;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Presentation.Controllers;

[ApiController]
[Route("api/shops")]
public class ShopController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromForm] CreateShopCommand command)
    {
        try
        {
            var result = await mediator.Send(command);
            result.Links = LinkFactory.ForShop(result.Id);
            return Created($"/api/shops/{result.Id}", result);
        }
        catch (ConflictException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPut("{shopId:guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid shopId, [FromForm] UpdateShopDto dto)
    {
        try
        {
            var result = await mediator.Send(new UpdateShopCommand() { Id = shopId, ShopDto = dto });
            result.Links = LinkFactory.ForShop(result.Id);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ConflictException ex)
        {
            return Conflict(ex.Message);
        }
    }
    
    [HttpDelete("{shopId:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid shopId)
    {
        try
        {
            await mediator.Send(new DeleteShopCommand() { Id = shopId });
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpGet("{shopId:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid shopId)
    {
        try
        {
            var result = await mediator.Send(new GetShopQuery(shopId));
            result.Links = LinkFactory.ForShop(result.Id);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{shopId:guid}/summary")]
    public async Task<IActionResult> GetSummaryAsync([FromRoute] Guid shopId)
    {
        try
        {
            var result = await mediator.Send(new GetShopSummaryQuery(shopId));
            result.Links = LinkFactory.ForShopSummary(result.Id);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        if (page < 1 || pageSize < 1 || pageSize > 50)
        {
            return BadRequest("page must be >= 1 and pageSize must be between 1 and 50.");
        }

        var result = await mediator.Send(new GetAllShopsQuery { Page = page, PageSize = pageSize, Search = search });
        foreach (var item in result.Items)
        {
            item.Links = LinkFactory.ForShop(item.Id);
        }

        result.Links = LinkFactory.ForPage(Request, result.Page, result.TotalPages);
        return Ok(result);
    }
}