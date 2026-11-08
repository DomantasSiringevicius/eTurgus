using Backend.Domain.Entities;

namespace Backend.Domain.IRepositories;

public interface IUserRepository
{
    Task<Guid> CreateAsync(User user);

    Task<User?> GetByIdAsync(Guid id);

    Task<User?> GetByEmailAsync(string email);
}