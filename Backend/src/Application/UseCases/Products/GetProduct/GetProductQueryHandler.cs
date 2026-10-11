using Backend.Application.Dtos.Product;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Products.GetProduct;

public class GetProductQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetProductQuery, ProductDto>
{
    public async Task<ProductDto> Handle(GetProductQuery query, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(query.ShopId, query.ProductId)
               ?? throw new Exception($"Product with id '{query.ProductId}' not found in shop '{query.ShopId}'.");

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Quantity = product.Quantity,
            Description = product.Description,
            PictureUri = product.PictureUri ?? string.Empty,
            ShopId = product.ShopId
        };
    }
}