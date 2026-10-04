using Backend.Application.Dtos.Review;
using MediatR;

namespace Backend.Application.UseCases.Reviews.CreateReview;

public class CreateReviewCommand : IRequest<ReviewDto>
{
    public required string Title { get; set; }
    
    public required string Content { get; set; }
    
    public required int Rating { get; set; }
    
    public required string Author { get; set; }
    
    public required Guid ProductId { get; set; }
}
