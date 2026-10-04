using Backend.Application.Dtos.Review;
using MediatR;

namespace Backend.Application.UseCases.Reviews.GetReview;

public class GetReviewQuery(Guid id) : IRequest<ReviewDto>
{
    public Guid Id { get; } = id;
}
