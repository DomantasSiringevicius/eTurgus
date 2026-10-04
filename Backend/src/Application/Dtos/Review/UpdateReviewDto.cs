namespace Backend.Application.Dtos.Review;

public class UpdateReviewDto
{
    public required string Title { get; set; }
    
    public required string Content { get; set; }
    
    public required int Rating { get; set; }
    
    public required string Author { get; set; }
}
