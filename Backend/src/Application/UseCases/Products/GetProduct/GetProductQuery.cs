using Backend.Application.Dtos.Product;
using MediatR;

namespace Backend.Application.UseCases.Products.GetProduct;

public class GetProductQuery(Guid shopId, Guid productId) : IRequest<ProductDto>
{
    public Guid ShopId { get; } = shopId;

    public Guid ProductId { get; } = productId;
}