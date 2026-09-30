using Backend.Application.Dtos.Product;
using MediatR;

namespace Backend.Application.UseCases.Products.UpdateProduct;

public class UpdateProductCommand : IRequest<ProductDto>
{
    public required string Name { get; set; }
    
    public required UpdateProductDto ProductDto { get; set; }
}