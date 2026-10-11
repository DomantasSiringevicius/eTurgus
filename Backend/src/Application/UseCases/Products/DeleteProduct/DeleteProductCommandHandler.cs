using Backend.Domain.Entities;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Products.DeleteProduct;

public class DeleteProductCommandHandler(IProductRepository productRepository) : IRequestHandler<DeleteProductCommand, Product>
{
    public async Task<Product> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(command.ShopId, command.ProductId);

        if(product is null)
        {
            throw new Exception($"Product with id '{command.ProductId}' not found in shop '{command.ShopId}'.");
        }

        var deletedProduct = await productRepository.DeleteAsync(product);

        if(deletedProduct is null)
        {
            throw new Exception($"Failed to delete product with id '{command.ProductId}'.");
        }

        return deletedProduct;
    }
}