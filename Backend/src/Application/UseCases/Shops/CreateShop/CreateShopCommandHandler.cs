using Backend.Domain.Entities;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Shops.CreateShop;

public class CreateShopCommandHandler(IShopRepository shopRepository) : IRequestHandler<CreateShopCommand, string?>
{
    public async Task<string?> Handle(CreateShopCommand command, CancellationToken cancellationToken)
    {
        var shopInDb = await shopRepository.GetByNameAsync(command.Name);
        
        if (shopInDb is not null)
        {
            throw new Exception($"Shop with name '{command.Name}' already exists.");
        }

        var shop = new Shop
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            ContactEmail = command.ContactEmail,
            ContactPhone = command.ContactPhone,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Products = new List<Product>()
        };

        await shopRepository.CreateAsync(shop);
        
        return shop.Name;
    }
}
