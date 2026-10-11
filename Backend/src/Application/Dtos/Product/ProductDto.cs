namespace Backend.Application.Dtos.Product;

public class ProductDto
{
    public required Guid Id { get; set; }
    
    public required string Name { get; set; }
    
    public required decimal Price { get; set; }
    
    public required int Quantity { get; set; }
    
    public required string Description { get; set; }
    
    public required string PictureUri { get; set; }
    
    public required Guid ShopId { get; set; }

    public List<LinkDto> Links { get; set; } = new List<LinkDto>();
}