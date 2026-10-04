using Backend.Domain.Entities;

namespace Backend.Domain.IRepositories;

public interface IShopRepository
{
    Task<string?> CreateAsync(Shop shop);
    Task<Shop?> GetByNameAsync(string name);
    Task<List<Shop>> GetAllAsync();
    Task<Shop?> UpdateAsync(Shop shop);
    Task<Shop?> DeleteAsync(Shop shop);
}