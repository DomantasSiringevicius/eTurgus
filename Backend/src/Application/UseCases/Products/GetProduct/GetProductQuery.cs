using Backend.Application.Dtos.Product;
using Backend.Domain.Entities;
using MediatR;

namespace Backend.Application.UseCases.Products.GetProduct;

public class GetProductQuery(string name) : IRequest<ProductDto>
{
    public string Name { get; } = name;
}