using Backend.Application.Dtos.Shop;
using MediatR;

namespace Backend.Application.UseCases.Shops.CreateShop;

public class CreateShopCommand : IRequest<string?>
{
    public required string Name { get; set; }
    
    public required string Description { get; set; }
    
    public required string ContactEmail { get; set; }
    
    public required string ContactPhone { get; set; }
}
