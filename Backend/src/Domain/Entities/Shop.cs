namespace Backend.Domain.Entities;

public class Shop : Entity
{
    public required string Name { get; set; }
    
    public required string Description { get; set; }
    
    public required string ContactEmail { get; set; }
    
    public required string ContactPhone { get; set; } 
    
    public Guid? OwnerId { get; set; }
    
    public User? Owner { get; set; }
    
    public List<Product> Products { get; set; } = new List<Product>();
}