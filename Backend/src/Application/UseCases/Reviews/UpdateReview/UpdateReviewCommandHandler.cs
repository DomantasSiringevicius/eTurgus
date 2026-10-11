using Backend.Application.Exceptions;
using Backend.Application.Dtos.Review;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Reviews.UpdateReview;

public class UpdateReviewCommandHandler(IReviewRepository reviewRepository)
    : IRequestHandler<UpdateReviewCommand, ReviewDto>
{
    public async Task<ReviewDto> Handle(UpdateReviewCommand command, CancellationToken cancellationToken)
    {
        var review = await reviewRepository.GetByIdAsync(command.ShopId, command.ProductId, command.Id)
               ?? throw new NotFoundException($"Review with id '{command.Id}' not found for product '{command.ProductId}'.");

        if (command.ReviewDto.Rating < 1 || command.ReviewDto.Rating > 5)
        {
            throw new ValidationException("Rating must be between 1 and 5.");
        }

        review.Title = command.ReviewDto.Title;
        review.Content = command.ReviewDto.Content;
        review.Rating = command.ReviewDto.Rating;
        review.Author = command.ReviewDto.Author;
        review.UpdatedAt = DateTime.Now;

        var updatedReview = await reviewRepository.UpdateAsync(review);

        if (updatedReview == null)
        {
            throw new NotFoundException("Error updating review");
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
