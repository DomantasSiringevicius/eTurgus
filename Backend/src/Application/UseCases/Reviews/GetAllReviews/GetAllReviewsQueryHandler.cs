using Backend.Application.Dtos;
using Backend.Application.Dtos.Review;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Reviews.GetAllReviews;

public class GetAllReviewsQueryHandler(IReviewRepository reviewRepository)
    : IRequestHandler<GetAllReviewsQuery, PagedResult<ReviewDto>>
{
    public async Task<PagedResult<ReviewDto>> Handle(GetAllReviewsQuery query, CancellationToken cancellationToken)
    {
        var (reviews, totalCount) = await reviewRepository.GetPagedAsync(
            query.Page, query.PageSize, query.MinRating, query.ProductId);

        return new PagedResult<ReviewDto>
        {
            Items = reviews.Select(review => new ReviewDto
            {
                Id = review.Id,
                Title = review.Title,
                Content = review.Content,
                Rating = review.Rating,
                Author = review.Author,
                ProductId = review.ProductId
            }).ToList(),
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }
}
