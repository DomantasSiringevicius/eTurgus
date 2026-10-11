using Backend.Application.Dtos;
using Backend.Application.Dtos.Product;
using Backend.Domain.Entities;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Products.GetAllProducts;

public class GetAllProductsQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetAllProductsQuery, PagedResult<ProductDto>>
{
    public async Task<PagedResult<ProductDto>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        var (products, totalCount) = await productRepository.GetPagedAsync(query.Page, query.PageSize);

        return new PagedResult<ProductDto>
        {
            Items = products.Select(product => new ProductDto
            {
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                Description = product.Description,
                PictureUri = product.PictureUri ?? string.Empty,
                ShopId = product.ShopId
            }).ToList(),
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }
}