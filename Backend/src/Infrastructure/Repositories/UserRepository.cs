using Backend.Domain.Entities;
using Backend.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task<Guid> CreateAsync(User user)
    {
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        return user.Id;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<(List<User> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? role)
    {
        var query = dbContext.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(role))
        {
            query = query.Where(u => u.Role == role);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(u => u.Email)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task UpdateAsync(User user)
    {
        user.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        dbContext.Users.Remove(user);

        await dbContext.SaveChangesAsync();
    }
}