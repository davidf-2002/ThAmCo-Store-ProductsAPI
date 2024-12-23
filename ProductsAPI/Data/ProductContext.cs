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

        ConfigureProductModel(modelBuilder);
        SeedProductData(modelBuilder);
    }

    private void ConfigureProductModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id); 
            entity.Property(p => p.Id).ValueGeneratedOnAdd(); 

            entity.Property(p => p.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(p => p.Description)
                  .IsRequired()
                  .HasMaxLength(500);

            entity.Property(p => p.Price)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

            entity.Property(p => p.StockStatus)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(p => p.LastUpdated)
                .HasColumnType("datetime2")
                .ValueGeneratedOnAddOrUpdate() 
                .HasDefaultValueSql("GETDATE()");
        });
    }

    private void SeedProductData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "T-shirt", Description = "Cotton white", Price = 15.99m, StockStatus = "In Stock", LastUpdated = DateTime.UtcNow},
            new Product { Id = 2, Name = "Running Shoes", Description = "Best for marathons", Price = 50.00m, StockStatus = "In Stock", LastUpdated = DateTime.UtcNow },
            new Product { Id = 3, Name = "Baseball Cap", Description = "Adjustable size", Price = 12.50m, StockStatus = "Out of Stock", LastUpdated = DateTime.UtcNow}
        );
    }
}
