using Microsoft.EntityFrameworkCore;
using ProductsAPI.Models;
using ProductsAPI.Data;
using System.Linq;

namespace ProductsAPI.Data;

public static class ProductInitialiser
{
    public static void Initialise(ProductContext context)
    {
        context.Database.EnsureCreated(); 

        // Check if any data exists in the categories table
        if (!context.Categories.Any())
        {
            // Seed Category data
            context.Categories.AddRange(
                new Category { Id = 1, Name = "Clothing" },
                new Category { Id = 2, Name = "Footwear" },
                new Category { Id = 3, Name = "Accessories" }
            );
            context.SaveChanges();
        }

        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product { Id = 1, Name = "T-shirt", Description = "Cotton white", Price = 15.99m, StockLevel = 2, CategoryId = 1 },
                new Product { Id = 2, Name = "Running Shoes", Description = "Best for marathons", Price = 50.00m, StockLevel = 7, CategoryId = 2 },
                new Product { Id = 3, Name = "Baseball Cap", Description = "Adjustable size", Price = 12.50m, StockLevel = 0, CategoryId = 3}
            );
            context.SaveChanges();
        }

        if (!context.Suppliers.Any())
        {
            context.Suppliers.AddRange(
                new Supplier { Id = 1, Name = "Global Textiles Inc", Email = "contact@globaltextiles.com" },
                new Supplier { Id = 2, Name = "Footwear Co.", Email = "info@footwearco.com" },
                new Supplier { Id = 3, Name = "Headgear Ltd.", Email = "support@headgearltd.com" }
            );
            context.SaveChanges();
        }

        if (!context.ProductSuppliers.Any())
        {
            context.ProductSuppliers.AddRange(
                new ProductSupplier { ProductId = 1, SupplierId = 1 },
                new ProductSupplier { ProductId = 2, SupplierId = 2 },
                new ProductSupplier { ProductId = 3, SupplierId = 3 },
                new ProductSupplier { ProductId = 1, SupplierId = 3 }
            );
            context.SaveChanges();
        }
    }
}