using Backend.Application.Dtos;
using Backend.Application.Dtos.Product;
using MediatR;

namespace Backend.Application.UseCases.Products.GetAllProducts;

public class GetAllProductsQuery : IRequest<PagedResult<ProductDto>>
{
    public required Guid ShopId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}