using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Reviews.DeleteReview;

public class DeleteReviewCommandHandler(IReviewRepository reviewRepository)
    : IRequestHandler<DeleteReviewCommand, Guid?>
{
    public async Task<Guid?> Handle(DeleteReviewCommand command, CancellationToken cancellationToken)
    {
        var review = await reviewRepository.GetByIdAsync(command.Id)
               ?? throw new Exception($"Review with id '{command.Id}' not found.");

        var result = await reviewRepository.DeleteAsync(review);

        return result?.Id;
    }
}
