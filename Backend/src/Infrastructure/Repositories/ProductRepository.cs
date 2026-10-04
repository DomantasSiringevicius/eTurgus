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

    public async Task<Product?> GetByNameAsync(string name)
    {
       return await dbContext.Products.FirstOrDefaultAsync(p => p.Name.ToLower().Equals(name.ToLower()));
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

        if (productInDb.Price < 0)
        {
            throw new Exception("Price cannot be negative");
        }

        if (productInDb.Quantity < 0)
        {
            throw new Exception("Quantity cannot be negative");
        }
        
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
}