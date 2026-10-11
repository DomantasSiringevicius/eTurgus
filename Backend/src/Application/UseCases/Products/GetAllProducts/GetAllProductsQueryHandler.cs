using Backend.Application.Exceptions;
using Backend.Application.Dtos;
using Backend.Application.Dtos.Product;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Products.GetAllProducts;

public class GetAllProductsQueryHandler(IProductRepository productRepository, IShopRepository shopRepository)
    : IRequestHandler<GetAllProductsQuery, PagedResult<ProductDto>>
{
    public async Task<PagedResult<ProductDto>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        var shop = await shopRepository.GetByIdAsync(query.ShopId)
               ?? throw new NotFoundException($"Shop with id '{query.ShopId}' not found.");

        var (products, totalCount) = await productRepository.GetPagedAsync(
            shop.Id, query.Page, query.PageSize, query.Search, query.MinPrice, query.MaxPrice);

        return new PagedResult<ProductDto>
        {
            Items = products.Select(product => new ProductDto
            {
                Id = product.Id,
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