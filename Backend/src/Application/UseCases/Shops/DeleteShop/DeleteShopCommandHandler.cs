using Backend.Domain.IRepositories;
using MediatR;

namespace Backend.Application.UseCases.Shops.DeleteShop;

public class DeleteShopCommandHandler(IShopRepository shopRepository)
    : IRequestHandler<DeleteShopCommand, string?>
{
    public async Task<string?> Handle(DeleteShopCommand command, CancellationToken cancellationToken)
    {
        var shop = await shopRepository.GetByNameAsync(command.Name);

        if (shop is null)
        {
            throw new Exception($"Shop with name '{command.Name}' not found.");
        }

        var result = await shopRepository.DeleteAsync(shop);

        return result?.Name;
    }
}
