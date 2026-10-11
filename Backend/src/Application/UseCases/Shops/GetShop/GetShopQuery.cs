using Backend.Application.Dtos.Shop;
using MediatR;

namespace Backend.Application.UseCases.Shops.GetShop;

public class GetShopQuery(Guid id) : IRequest<ShopDto>
{
    public Guid Id { get; } = id;
}
