using Backend.Application.Dtos.Product;
using Backend.Application.UseCases.Products.CreateProduct;
using Backend.Application.UseCases.Products.DeleteProduct;
using Backend.Application.UseCases.Products.GetAllProducts;
using Backend.Application.UseCases.Products.UpdateProduct;
using Backend.Application.UseCases.Products.GetProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Presentation.Controllers;

[Route("api/[controller]")]
public class ProductController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromForm] CreateProductCommand command)
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
    public async Task<IActionResult> UpdateAsync([FromRoute] string name, [FromForm] UpdateProductDto dto)
    {
        try
        {
            var result = await mediator.Send(new UpdateProductCommand {Name = name, ProductDto = dto});
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
            var result = await mediator.Send(new DeleteProductCommand { Name = name });
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
            var result = await mediator.Send(new GetProductQuery(name));
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
            var result = await mediator.Send(new GetAllProductsQuery());
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
}