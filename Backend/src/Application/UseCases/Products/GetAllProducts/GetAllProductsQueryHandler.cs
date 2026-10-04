using Backend.Application.Dtos.Product;
using Backend.Domain.Entities;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Products.GetAllProducts;

public class GetAllProductsQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
{
    public async Task<List<ProductDto>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetAllAsync();

        return products.Select(product => new ProductDto
        {
            Name = product.Name,
            Price = product.Price,
            Quantity = product.Quantity,
            Description = product.Description,
            PictureUri = product.PictureUri ?? string.Empty,
            ShopId = product.ShopId
        }).ToList();
    }
}