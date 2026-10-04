using Backend.Application.Dtos.Product;
using Backend.Domain.Entities;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Products.GetProduct;

public class GetProductQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetProductQuery, ProductDto>
{
    public async Task<ProductDto> Handle(GetProductQuery query, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByNameAsync(query.Name)
               ?? throw new Exception($"Product with name '{query.Name}' not found.");

        return new ProductDto
        {
            Name = product.Name,
            Price = product.Price,
            Quantity = product.Quantity,
            Description = product.Description,
            PictureUri = product.PictureUri ?? string.Empty,
            ShopId = product.ShopId
        };
    }
}