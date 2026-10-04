using MediatR;

namespace Backend.Application.UseCases.Shops.DeleteShop;

public class DeleteShopCommand : IRequest<string?>
{
    public required string Name { get; set; }
}
