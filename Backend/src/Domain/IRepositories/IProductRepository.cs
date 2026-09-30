using Backend.Domain.Entities;

namespace Backend.Domain.IRepositories;

public interface IProductRepository
{
    Task<string?> CreateAsync(Product product);
    
    Task<Product?> GetByNameAsync(string name);
    
    Task<Product?> UpdateAsync(Product updatedProduct);
    
    Task<Product?> DeleteAsync(Product product);
    
    Task<List<Product>> GetAllAsync();
}