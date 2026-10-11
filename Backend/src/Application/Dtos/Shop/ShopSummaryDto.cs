namespace Backend.Application.Dtos.Shop;

public class ShopSummaryDto
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public required string ContactEmail { get; set; }

    public required string ContactPhone { get; set; }

    public required int ProductCount { get; set; }

    public required int TotalQuantity { get; set; }

    public required int ReviewCount { get; set; }

    public required double? AverageRating { get; set; }

    public List<LinkDto> Links { get; set; } = new List<LinkDto>();
}
