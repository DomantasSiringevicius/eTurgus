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
        product.ShopId = command.ProductDto.ShopId;
        
        var updateProduct = await productRepository.UpdateAsync(product);

        if (updateProduct is null)
        {
            throw new Exception("Product not found");
        }

        return new ProductDto
        {
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
        var product = await productRepository.GetByNameAsync(command.Name);

        if (product is null)
        {
            throw new Exception($"Product with name '{command.Name}' not found.");
        }

        return product;
    }
}