using Backend.Application.Dtos;
using Backend.Application.Dtos.Review;
using MediatR;

namespace Backend.Application.UseCases.Reviews.GetAllReviews;

public class GetAllReviewsQuery : IRequest<PagedResult<ReviewDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}