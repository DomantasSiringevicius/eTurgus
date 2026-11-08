using MediatR;

namespace Backend.Application.UseCases.Reviews.DeleteReview;

public class DeleteReviewCommand : IRequest<Guid?>
{
    public required Guid ShopId { get; set; }
    public required Guid ProductId { get; set; }
    public required Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public required string UserRole { get; set; }
}