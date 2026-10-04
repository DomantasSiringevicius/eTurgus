using Backend.Application.Dtos.Product;
using Backend.Domain.Entities;
using MediatR;

namespace Backend.Application.UseCases.Products.GetAllProducts;

public class GetAllProductsQuery : IRequest<List<ProductDto>>
{
    
}