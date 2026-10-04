using Backend.Domain.Entities;

namespace Backend.Domain.IRepositories;

public interface IReviewRepository
{
    Task<Guid> CreateAsync(Review review);
    Task<Review?> GetByIdAsync(Guid id);
    Task<List<Review>> GetByProductIdAsync(Guid productId);
    Task<List<Review>> GetAllAsync();
    Task<Review?> UpdateAsync(Review review);
    Task<Review?> DeleteAsync(Review review);
}
