using Backend.Domain.Entities;

namespace Backend.Domain.IRepositories;

public interface IReviewRepository
{
    Task<Guid> CreateAsync(Review review);
    Task<Review?> GetByIdAsync(Guid shopId, Guid productId, Guid reviewId);
    Task<List<Review>> GetByProductIdAsync(Guid productId);
    Task<List<Review>> GetAllAsync();
    Task<Review?> UpdateAsync(Review review);
    Task<Review?> DeleteAsync(Review review);
    Task<(int Count, double? AverageRating)> GetStatsByShopAsync(Guid shopId);
    Task<(List<Review> Items, int TotalCount)> GetPagedAsync(Guid productId, int page, int pageSize, int? minRating);
}
