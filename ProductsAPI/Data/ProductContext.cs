using Microsoft.EntityFrameworkCore;
using ProductsAPI.Models;

namespace ProductsAPI.Data;

public class ProductContext : DbContext
{
    public ProductContext(DbContextOptions<ProductContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // modelBuilder.Entity<Product>(x =>
        // {
        //     x.Property(p => p.Name).IsRequired();
        //     x.Property(p => p.Description).IsRequired();
        //     x.HasOne(p => p.Suppliers)           
        //     .WithMany(s => s.Products)
        //     .HasForeignKey(p => p.SupplierId);
        // });

        modelBuilder.Entity<Product>(entity =>
        {
            // entity.Property(e => e.Price)
            //       .HasColumnType("decimal(18,2)"); // Specify decimal with precision and scale
        });

        // Seed data for Products
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "T-shirt", Description = "Cotton white", Price = 15.99m, StockStatus = "In Stock", LastUpdated = DateTime.UtcNow},
            new Product { Id = 2, Name = "Running Shoes", Description = "Best for marathons", Price = 50.00m, StockStatus = "In Stock", LastUpdated = DateTime.UtcNow },
            new Product { Id = 3, Name = "Baseball Cap", Description = "Adjustable size", Price = 12.50m, StockStatus = "Out of Stock", LastUpdated = DateTime.UtcNow}
        );
    }

}