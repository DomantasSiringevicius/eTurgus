using Backend.Application.Dtos.Review;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Reviews.GetAllReviews;

public class GetAllReviewsQueryHandler(IReviewRepository reviewRepository)
    : IRequestHandler<GetAllReviewsQuery, List<ReviewDto>>
{
    public async Task<List<ReviewDto>> Handle(GetAllReviewsQuery query, CancellationToken cancellationToken)
    {
        var reviews = await reviewRepository.GetAllAsync();

        return reviews.Select(review => new ReviewDto
        {
            Id = review.Id,
            Title = review.Title,
            Content = review.Content,
            Rating = review.Rating,
            Author = review.Author,
            ProductId = review.ProductId
        }).ToList();
    }
}
