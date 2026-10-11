using Backend.Application.Dtos.Review;
using MediatR;

namespace Backend.Application.UseCases.Reviews.GetReview;

public class GetReviewQuery(Guid shopId, Guid productId, Guid id) : IRequest<ReviewDto>
{
    public Guid ShopId { get; } = shopId;
    public Guid ProductId { get; } = productId;
    public Guid Id { get; } = id;
}
