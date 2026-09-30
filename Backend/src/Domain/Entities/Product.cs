namespace Backend.Domain.Entities;

public class Product : Entity
{
    public required string Name { get; set; }
    
    public required decimal Price { get; set; }
    
    public required int Quantity { get; set; }
    
    public required string Description { get; set; }
    
    public required string? PictureUri { get; set; }
}