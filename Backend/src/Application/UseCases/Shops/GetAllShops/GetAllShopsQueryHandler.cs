using Backend.Application.Dtos.Shop;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Shops.GetAllShops;

public class GetAllShopsQueryHandler(IShopRepository shopRepository)
    : IRequestHandler<GetAllShopsQuery, List<ShopDto>>
{
    public async Task<List<ShopDto>> Handle(GetAllShopsQuery query, CancellationToken cancellationToken)
    {
        var shops = await shopRepository.GetAllAsync();

        return shops.Select(shop => new ShopDto
        {
            Id = shop.Id,
            Name = shop.Name,
            Description = shop.Description,
            ContactEmail = shop.ContactEmail,
            ContactPhone = shop.ContactPhone
        }).ToList();
    }
}
