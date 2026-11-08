using Backend.Application.Exceptions;
using Backend.Domain.Entities;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Products.DeleteProduct;

public class DeleteProductCommandHandler(IProductRepository productRepository, IShopRepository shopRepository)
    : IRequestHandler<DeleteProductCommand, Product>
{
    public async Task<Product> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var shop = await shopRepository.GetByIdAsync(command.ShopId);

        if (shop is null)
        {
            throw new NotFoundException($"Shop with id '{command.ShopId}' not found.");
        }

        if (shop.OwnerId != command.UserId && command.UserRole != Roles.Admin)
        {
            throw new ForbiddenException("Only the shop owner or an admin can change this shop.");
        }

        var product = await productRepository.GetByIdAsync(command.ShopId, command.ProductId);

        if(product is null)
        {
            throw new NotFoundException($"Product with id '{command.ProductId}' not found in shop '{command.ShopId}'.");
        }

        var deletedProduct = await productRepository.DeleteAsync(product);

        if(deletedProduct is null)
        {
            throw new NotFoundException($"Failed to delete product with id '{command.ProductId}'.");
        }

        return deletedProduct;
    }
}