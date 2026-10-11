using Backend.Application.Dtos.Product;
using MediatR;

namespace Backend.Application.UseCases.Products.UpdateProduct;

public class UpdateProductCommand : IRequest<ProductDto>
{
    public required Guid ShopId { get; set; }

    public required Guid ProductId { get; set; }

    public required UpdateProductDto ProductDto { get; set; }
}