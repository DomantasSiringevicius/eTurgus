using Backend.Domain.Entities;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Products.CreateProduct;

public class CreateProductCommandHandler(IProductRepository productRepository) : IRequestHandler<CreateProductCommand, string?>
{
    public async Task<string?> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var productInDb = await productRepository.GetByNameAsync(command.Name);
        
        if(productInDb is not null)
        {
            throw new Exception($"Product with name '{command.Name}' already exists.");
        }

        if (command.Price < 0)
        {
            throw new Exception("Price cannot be negative");
        }
        
        if (command.Quantity < 0)
        {
            throw new Exception("Quantity cannot be negative");
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

       var result = await productRepository.CreateAsync(product);
       
       return result;
    }
}