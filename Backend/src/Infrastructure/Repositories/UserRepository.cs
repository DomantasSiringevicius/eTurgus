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
}