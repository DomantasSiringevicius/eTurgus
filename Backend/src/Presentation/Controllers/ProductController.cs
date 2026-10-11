using Backend.Application.Dtos.Product;
using Backend.Application.Exceptions;
using Backend.Application.UseCases.Products.CreateProduct;
using Backend.Application.UseCases.Products.DeleteProduct;
using Backend.Application.UseCases.Products.GetAllProducts;
using Backend.Application.UseCases.Products.UpdateProduct;
using Backend.Application.UseCases.Products.GetProduct;
using Backend.Presentation.Hypermedia;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Presentation.Controllers;

[ApiController]
[Route("api/shops/{shopId:guid}/products")]
public class ProductController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromRoute] Guid shopId, [FromForm] CreateProductDto dto)
    {
        try
        {
            var result = await mediator.Send(new CreateProductCommand
            {
                Name = dto.Name,
                Price = dto.Price,
                Quantity = dto.Quantity,
                Description = dto.Description,
                PictureUri = dto.PictureUri,
                ShopId = shopId
            });
            result.Links = LinkFactory.ForProduct(result.ShopId, result.Id);
            return Created($"/api/shops/{result.ShopId}/products/{result.Id}", result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
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
    
    [HttpPut("{productId:guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid shopId, [FromRoute] Guid productId,
        [FromForm] UpdateProductDto dto)
    {
        try
        {
            var result = await mediator.Send(new UpdateProductCommand
            {
                ShopId = shopId,
                ProductId = productId,
                ProductDto = dto
            });
            result.Links = LinkFactory.ForProduct(result.ShopId, result.Id);
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
        catch (ValidationException ex)
        {
            return UnprocessableEntity(ex.Message);
        }
    }

    [HttpDelete("{productId:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid shopId, [FromRoute] Guid productId)
    {
        try
        {
            await mediator.Send(new DeleteProductCommand { ShopId = shopId, ProductId = productId });
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{productId:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid shopId, [FromRoute] Guid productId)
    {
        try
        {
            var result = await mediator.Send(new GetProductQuery(shopId, productId));
            result.Links = LinkFactory.ForProduct(result.ShopId, result.Id);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromRoute] Guid shopId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null)
    {
        if (page < 1 || pageSize < 1 || pageSize > 50)
        {
            return BadRequest("page must be >= 1 and pageSize must be between 1 and 50.");
        }

        if (minPrice < 0 || maxPrice < 0 || minPrice > maxPrice)
        {
            return BadRequest("minPrice and maxPrice must be >= 0 and minPrice cannot be greater than maxPrice.");
        }

        try
        {
            var result = await mediator.Send(new GetAllProductsQuery
            {
                ShopId = shopId,
                Page = page,
                PageSize = pageSize,
                Search = search,
                MinPrice = minPrice,
                MaxPrice = maxPrice
            });
            foreach (var item in result.Items)
            {
                item.Links = LinkFactory.ForProduct(item.ShopId, item.Id);
            }

            result.Links = LinkFactory.ForPage(Request, result.Page, result.TotalPages);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
}