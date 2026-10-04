using Backend.Domain.Entities;
using Backend.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class ShopRepository(AppDbContext dbContext) : IShopRepository
{
    public async Task<string?> CreateAsync(Shop newShop)
    {
        await dbContext.Shops.AddAsync(newShop);
        await dbContext.SaveChangesAsync();

        return newShop.Name;
    }

    public async Task<Shop?> GetByNameAsync(string name)
    {
        return await dbContext.Shops.FirstOrDefaultAsync(s => s.Name.ToLower().Equals(name.ToLower()));
    }
    
    public async Task<List<Shop>> GetAllAsync()
    {
        return await dbContext.Shops.ToListAsync();
    }

    public async Task<Shop?> UpdateAsync(Shop shop)
    {
        var shopInDb = await dbContext.Shops.FirstOrDefaultAsync(s => s.Id.Equals(shop.Id));
        
        if (shopInDb is null)
        {
            return null;
        }
        
        shopInDb.Name = shop.Name;
        shopInDb.Description = shop.Description;
        shopInDb.ContactEmail = shop.ContactEmail;
        shopInDb.ContactPhone = shop.ContactPhone;
        shopInDb.UpdatedAt = DateTime.Now;
        
        await dbContext.SaveChangesAsync();
        
        return shopInDb;
    }
    
    public async Task<Shop?> DeleteAsync(Shop shop)
    {
        var shopInDb = dbContext.Shops.FirstOrDefault(s => s.Id.Equals(shop.Id));

        if (shopInDb is null)
        {
            return null;
        }

        dbContext.Shops.Remove(shopInDb);
        await dbContext.SaveChangesAsync();

        return shopInDb;
    }
}