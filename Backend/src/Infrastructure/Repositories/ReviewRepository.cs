using Backend.Domain.Entities;
using Backend.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class ReviewRepository(AppDbContext dbContext) : IReviewRepository
{
    public async Task<Guid> CreateAsync(Review review)
    {
        await dbContext.Reviews.AddAsync(review);
        await dbContext.SaveChangesAsync();
        return review.Id;
    }

    public async Task<Review?> GetByIdAsync(Guid shopId, Guid productId, Guid reviewId)
    {
        return await dbContext.Reviews.FirstOrDefaultAsync(r =>
            r.Id == reviewId && r.ProductId == productId && r.Product.ShopId == shopId);
    }

    public async Task<List<Review>> GetByProductIdAsync(Guid productId)
    {
        return await dbContext.Reviews.Where(r => r.ProductId == productId).ToListAsync();
    }

    public async Task<List<Review>> GetAllAsync()
    {
        return await dbContext.Reviews.ToListAsync();
    }

    public async Task<Review?> UpdateAsync(Review review)
    {
        var reviewInDb = await dbContext.Reviews.FirstOrDefaultAsync(r => r.Id == review.Id);
        if (reviewInDb == null) return null;

        reviewInDb.Title = review.Title;
        reviewInDb.Content = review.Content;
        reviewInDb.Rating = review.Rating;
        reviewInDb.Author = review.Author;
        reviewInDb.UpdatedAt = DateTime.Now;

        await dbContext.SaveChangesAsync();
        return reviewInDb;
    }

    public async Task<Review?> DeleteAsync(Review review)
    {
        var reviewInDb = await dbContext.Reviews.FirstOrDefaultAsync(r => r.Id == review.Id);
        if (reviewInDb == null) return null;

        dbContext.Reviews.Remove(reviewInDb);
        await dbContext.SaveChangesAsync();
        return reviewInDb;
    }
    
    public async Task<(List<Review> Items, int TotalCount)> GetPagedAsync(Guid productId, int page, int pageSize, int? minRating)
    {
        var query = dbContext.Reviews.Where(r => r.ProductId == productId);

        if (minRating is not null)
        {
            query = query.Where(r => r.Rating >= minRating);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(r => r.CreatedAt)
            .ThenBy(r => r.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
