namespace Backend.Application.Dtos.Shop;

public class UpdateShopDto
{
    public required string Name { get; set; }
    
    public required string Description { get; set; }
    
    public required string ContactEmail { get; set; }
    
    public required string ContactPhone { get; set; }
}
