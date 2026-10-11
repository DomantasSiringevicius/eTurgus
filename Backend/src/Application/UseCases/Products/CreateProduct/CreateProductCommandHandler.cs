using Backend.Application.Exceptions;
using Backend.Application.Dtos.Product;
using Backend.Domain.Entities;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Products.CreateProduct;

public class CreateProductCommandHandler(IProductRepository productRepository, IShopRepository shopRepository)
    : IRequestHandler<CreateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var shop = await shopRepository.GetByIdAsync(command.ShopId);

        if (shop is null)
        {
            throw new NotFoundException($"Shop with id '{command.ShopId}' not found.");
        }

        var productInDb = await productRepository.GetByNameAsync(command.ShopId, command.Name);

        if(productInDb is not null)
        {
            throw new ConflictException($"Product with name '{command.Name}' already exists in this shop.");
        }

        if (command.Price <= 0)
        {
            throw new ValidationException("Price must be greater than 0");
        }

        if (command.Quantity < 0)
        {
            throw new ValidationException("Quantity cannot be negative");
        }

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            Price = command.Price,
            Quantity = command.Quantity,
            PictureUri = command.PictureUri,
            ShopId = command.ShopId,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
        };

        await productRepository.CreateAsync(product);

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Quantity = product.Quantity,
            Description = product.Description,
            PictureUri = product.PictureUri ?? string.Empty,
            ShopId = product.ShopId
        };
    }
}