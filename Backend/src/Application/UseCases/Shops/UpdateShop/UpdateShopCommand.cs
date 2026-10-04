using Backend.Application.Dtos.Shop;
using MediatR;

namespace Backend.Application.UseCases.Shops.UpdateShop;

public class UpdateShopCommand : IRequest<ShopDto>
{
    public required string Name { get; set; }
    public required UpdateShopDto ShopDto { get; set; }
}
