using Backend.Application.Dtos.Shop;
using MediatR;

namespace Backend.Application.UseCases.Shops.GetAllShops;

public class GetAllShopsQuery : IRequest<List<ShopDto>>
{
}
