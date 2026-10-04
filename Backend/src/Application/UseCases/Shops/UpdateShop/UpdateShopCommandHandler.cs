using Backend.Application.Dtos.Shop;
using Backend.Domain.Entities;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Shops.UpdateShop;

public class UpdateShopCommandHandler(IShopRepository shopRepository)
    : IRequestHandler<UpdateShopCommand, ShopDto>
{
    public async Task<ShopDto> Handle(UpdateShopCommand command, CancellationToken cancellationToken)
    {
        var shop = await shopRepository.GetByNameAsync(command.Name);

        if (shop is null)
        {
            throw new Exception($"Shop with name '{command.Name}' not found.");
        }

        shop.Name = command.ShopDto.Name;
        shop.Description = command.ShopDto.Description;
        shop.ContactEmail = command.ShopDto.ContactEmail;
        shop.ContactPhone = command.ShopDto.ContactPhone;
        shop.UpdatedAt = DateTime.Now;

        var updatedShop = await shopRepository.UpdateAsync(shop);

        if (updatedShop is null)
        {
            throw new Exception("Shop not found");
        }

        return new ShopDto
        {
            Id = updatedShop.Id,
            Name = updatedShop.Name,
            Description = updatedShop.Description,
            ContactEmail = updatedShop.ContactEmail,
            ContactPhone = updatedShop.ContactPhone
        };
    }
}
