using Backend.Application.Exceptions;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Reviews.DeleteReview;

public class DeleteReviewCommandHandler(IReviewRepository reviewRepository)
    : IRequestHandler<DeleteReviewCommand, Guid?>
{
    public async Task<Guid?> Handle(DeleteReviewCommand command, CancellationToken cancellationToken)
    {
        var review = await reviewRepository.GetByIdAsync(command.ShopId, command.ProductId, command.Id)
               ?? throw new NotFoundException($"Review with id '{command.Id}' not found for product '{command.ProductId}'.");

        var result = await reviewRepository.DeleteAsync(review);

        return result?.Id;
    }
}
