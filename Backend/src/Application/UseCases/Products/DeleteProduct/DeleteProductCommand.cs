using Backend.Application.Dtos.Product;
using Backend.Domain.Entities;
using MediatR;

namespace Backend.Application.UseCases.Products.DeleteProduct;

public class DeleteProductCommand : IRequest<Product>
{
    public required string Name { get; set; }
}