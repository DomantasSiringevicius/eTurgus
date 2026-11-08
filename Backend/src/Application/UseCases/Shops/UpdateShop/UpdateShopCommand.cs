using Backend.Application.Dtos.Shop;
using MediatR;

namespace Backend.Application.UseCases.Shops.UpdateShop;

public class UpdateShopCommand : IRequest<ShopDto>
{
    public required Guid Id { get; set; }
    public required UpdateShopDto ShopDto { get; set; }
    public required Guid UserId { get; set; }

    public required string UserRole { get; set; }
}
