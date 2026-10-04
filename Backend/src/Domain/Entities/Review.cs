namespace Backend.Domain.Entities;

public class Review : Entity
{
    public required string Title { get; set; }
    
    public required string Content { get; set; }
    
    public required int Rating { get; set; }
    
    public required string Author { get; set; }
    
    public required Guid ProductId { get; set; }
    
    public Product Product { get; set; } = null!;
}
