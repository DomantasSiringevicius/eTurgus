using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Shop> Shops { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Name)
            .IsUnique();
        
        modelBuilder.Entity<Shop>()
            .HasIndex(s => s.Name)
            .IsUnique();

        modelBuilder.Entity<Shop>()
            .HasMany(s => s.Products)
            .WithOne(p => p.Shop)
            .HasForeignKey(p => p.ShopId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>()
            .HasMany(p => p.Reviews)
            .WithOne(r => r.Product)
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        
    }
    

}
