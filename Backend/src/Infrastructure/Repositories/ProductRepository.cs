using Backend.Domain.Entities;
using Backend.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class ProductRepository(AppDbContext dbContext) : IProductRepository
{
    public async Task<string?> CreateAsync(Product newProduct)
    {
        await dbContext.Products.AddAsync(newProduct);
        await dbContext.SaveChangesAsync();

        return newProduct.Name;
    }

    public async Task<Product?> GetByIdAsync(Guid shopId, Guid productId)
    {
        return await dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId && p.ShopId == shopId);
    }

    public async Task<Product?> GetByNameAsync(Guid shopId, string name)
    {
       return await dbContext.Products.FirstOrDefaultAsync(p =>
           p.ShopId == shopId && p.Name.ToLower().Equals(name.ToLower()));
    }

    public async Task<Product?> UpdateAsync(Product product)
    {
        var productInDb = await dbContext.Products.FirstOrDefaultAsync(p => p.Id.Equals(product.Id));

        if (productInDb is null)
        {
            return null;
        }
        
        productInDb.Name = product.Name;
        productInDb.Price = product.Price;
        productInDb.Quantity = product.Quantity;
        productInDb.Description = product.Description;
        productInDb.PictureUri = product.PictureUri;
        productInDb.ShopId = product.ShopId;
        productInDb.UpdatedAt = DateTime.Now;

        await dbContext.SaveChangesAsync();
        
        return productInDb;
    }

    public async Task<Product?> DeleteAsync(Product product)
    { 
        var productInDb = dbContext.Products.FirstOrDefault(p => p.Id.Equals(product.Id));

        if (productInDb is null)
        {
            return null;
        }

        dbContext.Products.Remove(productInDb);
        await dbContext.SaveChangesAsync();

        return productInDb;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await dbContext.Products.ToListAsync();
    }
    
    public async Task<(List<Product> Items, int TotalCount)> GetPagedAsync(Guid shopId, int page, int pageSize,
        string? search, decimal? minPrice, decimal? maxPrice)
    {
        var query = dbContext.Products.Where(p => p.ShopId == shopId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.Name.ToLower().Contains(search.ToLower()));
        }

        if (minPrice is not null)
        {
            query = query.Where(p => p.Price >= minPrice);
        }

        if (maxPrice is not null)
        {
            query = query.Where(p => p.Price <= maxPrice);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(int Count, int TotalQuantity)> GetStatsByShopAsync(Guid shopId)
    {
        var products = dbContext.Products.Where(p => p.ShopId == shopId);

        var count = await products.CountAsync();
        var totalQuantity = await products.SumAsync(p => p.Quantity);

        return (count, totalQuantity);
    }
}