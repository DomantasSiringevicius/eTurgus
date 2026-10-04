using Backend.Application.Dtos.Review;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Reviews.GetReview;

public class GetReviewQueryHandler(IReviewRepository reviewRepository)
    : IRequestHandler<GetReviewQuery, ReviewDto>
{
    public async Task<ReviewDto> Handle(GetReviewQuery query, CancellationToken cancellationToken)
    {
        var review = await reviewRepository.GetByIdAsync(query.Id)
               ?? throw new Exception($"Review with id '{query.Id}' not found.");

        return new ReviewDto
        {
            Id = review.Id,
            Title = review.Title,
            Content = review.Content,
            Rating = review.Rating,
            Author = review.Author,
            ProductId = review.ProductId
        };
    }
}
