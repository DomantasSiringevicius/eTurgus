using Backend.Application.Dtos.Shop;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Shops.GetShopSummary;

public class GetShopSummaryQueryHandler(
    IShopRepository shopRepository,
    IProductRepository productRepository,
    IReviewRepository reviewRepository)
    : IRequestHandler<GetShopSummaryQuery, ShopSummaryDto>
{
    public async Task<ShopSummaryDto> Handle(GetShopSummaryQuery query, CancellationToken cancellationToken)
    {
        var shop = await shopRepository.GetByIdAsync(query.Id)
               ?? throw new Exception($"Shop with id '{query.Id}' not found.");

        var (productCount, totalQuantity) = await productRepository.GetStatsByShopAsync(shop.Id);
        var (reviewCount, averageRating) = await reviewRepository.GetStatsByShopAsync(shop.Id);

        return new ShopSummaryDto
        {
            Id = shop.Id,
            Name = shop.Name,
            Description = shop.Description,
            ContactEmail = shop.ContactEmail,
            ContactPhone = shop.ContactPhone,
            ProductCount = productCount,
            TotalQuantity = totalQuantity,
            ReviewCount = reviewCount,
            AverageRating = averageRating is null ? null : Math.Round(averageRating.Value, 2)
        };
    }
}
