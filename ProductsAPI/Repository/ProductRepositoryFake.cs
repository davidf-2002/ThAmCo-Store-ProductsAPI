using System;
using ProductsAPI.Models;

namespace ProductsAPI.Repository;

public class ProductRepositoryFake : IProductRepository
{

    private readonly List<Product> _products = new List<Product>
    {
        new Product { Id = 1, Name = "T-shirt", Description = "Jack & Jones", Price = 11.50m, StockStatus = "In Stock", LastUpdated = new DateTime(2024, 11, 07)},
        new Product { Id = 2, Name = "Jeans", Description = "Armani", Price = 30.00m, StockStatus = "In Stock", LastUpdated = new DateTime(2024, 11, 07)},
        new Product { Id = 3, Name = "Hoody", Description = "Boss", Price = 20.99m, StockStatus = "Out of Stock", LastUpdated = new DateTime(2024, 11, 07)}    
    };

    public Task<IEnumerable<Product>> GetProductsAsync()
    {
        var products = _products.AsEnumerable();
        return Task.FromResult(products);
    }

    public Task<Product?> GetProductAsync(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product);
    }

    public Task<Product> AddProductAsync(Product product)
    {
        throw new NotImplementedException();    // TODO
        
    }

    public Task<Product> UpdateProductAsync(Product prodcut)
    {
        throw new NotImplementedException();    // TODO
    }

    public void DeleteProductAsync(int id)
    {
        throw new NotImplementedException();    // TODO
    }

}