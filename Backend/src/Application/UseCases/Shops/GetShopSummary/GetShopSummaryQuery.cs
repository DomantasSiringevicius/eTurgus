using Backend.Application.Dtos.Shop;
using MediatR;

namespace Backend.Application.UseCases.Shops.GetShopSummary;

public class GetShopSummaryQuery(Guid id) : IRequest<ShopSummaryDto>
{
    public Guid Id { get; } = id;
}
