using Backend.Application.Dtos;
using Backend.Application.Dtos.Shop;
using MediatR;

namespace Backend.Application.UseCases.Shops.GetAllShops;

public class GetAllShopsQuery : IRequest<PagedResult<ShopDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
}