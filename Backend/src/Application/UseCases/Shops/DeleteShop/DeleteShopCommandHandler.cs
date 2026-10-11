using Backend.Application.Exceptions;
using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Shops.DeleteShop;

public class DeleteShopCommandHandler(IShopRepository shopRepository)
    : IRequestHandler<DeleteShopCommand, string?>
{
    public async Task<string?> Handle(DeleteShopCommand command, CancellationToken cancellationToken)
    {
        var shop = await shopRepository.GetByIdAsync(command.Id);

        if (shop is null)
        {
            throw new NotFoundException($"Shop with id '{command.Id}' not found.");
        }

        var result = await shopRepository.DeleteAsync(shop);

        return result?.Name;
    }
}
