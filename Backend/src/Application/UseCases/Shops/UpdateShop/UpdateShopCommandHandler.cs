using Backend.Application.Exceptions;
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
        var shop = await shopRepository.GetByIdAsync(command.Id);

        if (shop is null)
        {
            throw new NotFoundException($"Shop with id '{command.Id}' not found.");
        }

        var shopWithSameName = await shopRepository.GetByNameAsync(command.ShopDto.Name);

        if (shopWithSameName is not null && shopWithSameName.Id != shop.Id)
        {
            throw new ConflictException($"Shop with name '{command.ShopDto.Name}' already exists.");
        }

        shop.Name = command.ShopDto.Name;
        shop.Description = command.ShopDto.Description;
        shop.ContactEmail = command.ShopDto.ContactEmail;
        shop.ContactPhone = command.ShopDto.ContactPhone;
        shop.UpdatedAt = DateTime.Now;

        var updatedShop = await shopRepository.UpdateAsync(shop);

        if (updatedShop is null)
        {
            throw new NotFoundException("Shop not found");
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
