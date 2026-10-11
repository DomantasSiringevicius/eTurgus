using MediatR;

namespace Backend.Application.UseCases.Reviews.DeleteReview;

public class DeleteReviewCommand : IRequest<Guid?>
{
    public required Guid Id { get; set; }
}