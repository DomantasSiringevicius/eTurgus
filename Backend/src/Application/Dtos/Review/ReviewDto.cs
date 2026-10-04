namespace Backend.Application.Dtos.Review;

public class ReviewDto
{
    public required Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public required string Content { get; set; }
    
    public required int Rating { get; set; }
    
    public required string Author { get; set; }
    
    public required Guid ProductId { get; set; }
}
