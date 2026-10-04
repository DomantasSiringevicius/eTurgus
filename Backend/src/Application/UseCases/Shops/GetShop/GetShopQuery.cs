using Backend.Application.Dtos.Shop;
using MediatR;

namespace Backend.Application.UseCases.Shops.GetShop;

public class GetShopQuery(string name) : IRequest<ShopDto>
{
    public string Name { get; } = name;
}
