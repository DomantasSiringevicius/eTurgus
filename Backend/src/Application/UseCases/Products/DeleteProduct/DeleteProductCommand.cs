using Backend.Domain.Entities;
using MediatR;

namespace Backend.Application.UseCases.Products.DeleteProduct;

public class DeleteProductCommand : IRequest<Product>
{
    public required Guid ShopId { get; set; }

    public required Guid ProductId { get; set; }
}