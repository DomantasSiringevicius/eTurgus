using Backend.Application.Exceptions;
using Backend.Application.Dtos.Review;
using Backend.Domain.Entities;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Reviews.CreateReview;

public class CreateReviewCommandHandler(IReviewRepository reviewRepository, IProductRepository productRepository) 
    : IRequestHandler<CreateReviewCommand, ReviewDto>
{
    public async Task<ReviewDto> Handle(CreateReviewCommand command, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(command.ShopId, command.ProductId)
               ?? throw new NotFoundException($"Product with id '{command.ProductId}' not found in shop '{command.ShopId}'.");

        if (command.Rating < 1 || command.Rating > 5)
        {
            throw new ValidationException("Rating must be between 1 and 5.");
        }

        var review = new Review
        {
            Id = Guid.NewGuid(),
            Title = command.Title,
            Content = command.Content,
            Rating = command.Rating,
            Author = command.Author,
            ProductId = product.Id,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        await reviewRepository.CreateAsync(review);
        
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
