using Backend.Domain.Entities;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Products.GetProduct;

public class GetProductQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetProductQuery, Product>
{
    public async Task<Product> Handle(GetProductQuery query, CancellationToken cancellationToken)
    {
        return await productRepository.GetByNameAsync(query.Name)
               ?? throw new Exception($"Product with name '{query.Name}' not found.");
    }
}