using Backend.Application.Dtos;
using Backend.Application.Dtos.Shop;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Shops.GetAllShops;

public class GetAllShopsQueryHandler(IShopRepository shopRepository)
    : IRequestHandler<GetAllShopsQuery, PagedResult<ShopDto>>
{
    public async Task<PagedResult<ShopDto>> Handle(GetAllShopsQuery query, CancellationToken cancellationToken)
    {
        var (shops, totalCount) = await shopRepository.GetPagedAsync(query.Page, query.PageSize, query.Search);

        return new PagedResult<ShopDto>
        {
            Items = shops.Select(shop => new ShopDto
            {
                Id = shop.Id,
                Name = shop.Name,
                Description = shop.Description,
                ContactEmail = shop.ContactEmail,
                ContactPhone = shop.ContactPhone
            }).ToList(),
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }
}
