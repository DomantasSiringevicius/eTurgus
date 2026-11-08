using MediatR;

namespace Backend.Application.UseCases.Shops.DeleteShop;

public class DeleteShopCommand : IRequest<string?>
{
    public required Guid Id { get; set; }
    public required Guid UserId { get; set; }

    public required string UserRole { get; set; }
}
