using Backend.Application.Dtos.Review;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Reviews.UpdateReview;

public class UpdateReviewCommandHandler(IReviewRepository reviewRepository)
    : IRequestHandler<UpdateReviewCommand, ReviewDto>
{
    public async Task<ReviewDto> Handle(UpdateReviewCommand command, CancellationToken cancellationToken)
    {
        var review = await reviewRepository.GetByIdAsync(command.Id)
               ?? throw new Exception($"Review with id '{command.Id}' not found.");

        review.Title = command.ReviewDto.Title;
        review.Content = command.ReviewDto.Content;
        review.Rating = command.ReviewDto.Rating;
        review.Author = command.ReviewDto.Author;
        review.UpdatedAt = DateTime.Now;

        var updatedReview = await reviewRepository.UpdateAsync(review);

        if (updatedReview == null)
        {
            throw new Exception("Error updating review");
        }

        return new ReviewDto
        {
            Id = updatedReview.Id,
            Title = updatedReview.Title,
            Content = updatedReview.Content,
            Rating = updatedReview.Rating,
            Author = updatedReview.Author,
            ProductId = updatedReview.ProductId
        };
    }
}
