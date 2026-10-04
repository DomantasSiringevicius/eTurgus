using Backend.Application.Dtos.Review;
using MediatR;

namespace Backend.Application.UseCases.Reviews.UpdateReview;

public class UpdateReviewCommand : IRequest<ReviewDto>
{
    public required Guid Id { get; set; }
    public required UpdateReviewDto ReviewDto { get; set; }
}
