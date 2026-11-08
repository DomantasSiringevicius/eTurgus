using Backend.Domain.Entities;
using MediatR;

namespace Backend.Application.UseCases.Products.DeleteProduct;

public class DeleteProductCommand : IRequest<Product>
{
    public required Guid ShopId { get; set; }

    public required Guid ProductId { get; set; }

    public required Guid UserId { get; set; }

    public required string UserRole { get; set; }
}