using Backend.Domain.Entities;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Products.DeleteProduct;

public class DeleteProductCommandHandler(IProductRepository productRepository) : IRequestHandler<DeleteProductCommand, Product>
{
    public async Task<Product> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByNameAsync(command.Name);
        
        if(product is null)
        {
            throw new Exception($"Product with name '{command.Name}' not found.");
        }
        
        var deletedProduct = await productRepository.DeleteAsync(product);
        
        if(deletedProduct is null)
        {
            throw new Exception($"Failed to delete product with name '{command.Name}'.");
        }

        return deletedProduct;
    }
}