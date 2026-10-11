using Backend.Domain.Entities;

namespace Backend.Domain.IRepositories;

public interface IProductRepository
{
    Task<string?> CreateAsync(Product product);
    
    Task<Product?> GetByIdAsync(Guid shopId, Guid productId);
    
    Task<Product?> GetByNameAsync(Guid shopId, string name);
    
    Task<Product?> UpdateAsync(Product updatedProduct);
    
    Task<Product?> DeleteAsync(Product product);
    
    Task<List<Product>> GetAllAsync();
    Task<(int Count, int TotalQuantity)> GetStatsByShopAsync(Guid shopId);
    Task<(List<Product> Items, int TotalCount)> GetPagedAsync(Guid shopId, int page, int pageSize, string? search, decimal? minPrice, decimal? maxPrice);
}