using Backend.Application.Exceptions;
using Backend.Application.Dtos;
using Backend.Application.Dtos.Review;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Reviews.GetAllReviews;

public class GetAllReviewsQueryHandler(IReviewRepository reviewRepository, IProductRepository productRepository)
    : IRequestHandler<GetAllReviewsQuery, PagedResult<ReviewDto>>
{
    public async Task<PagedResult<ReviewDto>> Handle(GetAllReviewsQuery query, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(query.ShopId, query.ProductId)
               ?? throw new NotFoundException($"Product with id '{query.ProductId}' not found in shop '{query.ShopId}'.");

        var (reviews, totalCount) = await reviewRepository.GetPagedAsync(
            product.Id, query.Page, query.PageSize, query.MinRating);

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
