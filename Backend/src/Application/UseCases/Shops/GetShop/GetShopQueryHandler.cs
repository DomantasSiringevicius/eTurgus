using Backend.Application.Dtos.Shop;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Shops.GetShop;

public class GetShopQueryHandler(IShopRepository shopRepository)
    : IRequestHandler<GetShopQuery, ShopDto>
{
    public async Task<ShopDto> Handle(GetShopQuery query, CancellationToken cancellationToken)
    {
        var shop = await shopRepository.GetByIdAsync(query.Id)
               ?? throw new Exception($"Shop with id '{query.Id}' not found.");

        return new ShopDto
        {
            Id = shop.Id,
            Name = shop.Name,
            Description = shop.Description,
            ContactEmail = shop.ContactEmail,
            ContactPhone = shop.ContactPhone
        };
    }
}
