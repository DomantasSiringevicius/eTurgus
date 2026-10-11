using Backend.Application.Exceptions;
using Backend.Application.Dtos.Review;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Reviews.GetReview;

public class GetReviewQueryHandler(IReviewRepository reviewRepository)
    : IRequestHandler<GetReviewQuery, ReviewDto>
{
    public async Task<ReviewDto> Handle(GetReviewQuery query, CancellationToken cancellationToken)
    {
        var review = await reviewRepository.GetByIdAsync(query.ShopId, query.ProductId, query.Id)
               ?? throw new NotFoundException($"Review with id '{query.Id}' not found for product '{query.ProductId}'.");

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
