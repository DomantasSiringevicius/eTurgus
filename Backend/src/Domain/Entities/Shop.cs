namespace Backend.Domain.Entities;

public class Shop : Entity
{
    public required string Name { get; set; }
    
    public required string Description { get; set; }
    
    //public required string OwnerId { get; set; } TODO: Add user entity
    
    public required string ContactEmail { get; set; }
    
    public required string ContactPhone { get; set; } 
    
    public List<Product> Products { get; set; } = new List<Product>();
}