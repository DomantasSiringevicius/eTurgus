using Backend.Application.Exceptions;
using Backend.Application.Dtos.Product;
using Backend.Domain.Entities;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Products.UpdateProduct;

public class UpdateProductCommandHandler(IProductRepository productRepository)
    : IRequestHandler<UpdateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await ValidateAsync(command);

        product.Name = command.ProductDto.Name;
        product.Price = command.ProductDto.Price;
        product.Quantity = command.ProductDto.Quantity;
        product.Description = command.ProductDto.Description;
        product.PictureUri = command.ProductDto.PictureUri;

        var updateProduct = await productRepository.UpdateAsync(product);

        if (updateProduct is null)
        {
            throw new NotFoundException("Product not found");
        }

        return new ProductDto
        {
            Id = updateProduct.Id,
            Name = updateProduct.Name,
            Price = updateProduct.Price,
            Quantity = updateProduct.Quantity,
            Description = updateProduct.Description,
            PictureUri = updateProduct.PictureUri!,
            ShopId = updateProduct.ShopId
        };
    }

    private async Task<Product> ValidateAsync(UpdateProductCommand command)
    {
        var product = await productRepository.GetByIdAsync(command.ShopId, command.ProductId);

        if (product is null)
        {
            throw new NotFoundException($"Product with id '{command.ProductId}' not found in shop '{command.ShopId}'.");
        }

        var productWithSameName = await productRepository.GetByNameAsync(command.ShopId, command.ProductDto.Name);

        if (productWithSameName is not null && productWithSameName.Id != product.Id)
        {
            throw new ConflictException($"Product with name '{command.ProductDto.Name}' already exists in this shop.");
        }

        if (command.ProductDto.Price <= 0)
        {
            throw new ValidationException("Price must be greater than 0");
        }

        if (command.ProductDto.Quantity < 0)
        {
            throw new ValidationException("Quantity cannot be negative");
        }

        return product;
    }
}