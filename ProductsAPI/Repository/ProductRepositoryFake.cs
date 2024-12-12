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

    public Task<Product?> AddProductAsync(Product product)
    {
        int newId = _products.Max(p => p.Id) + 1; 
        product.Id = newId;
        _products.Add(product);
        return Task.FromResult<Product?>(product);
    }

    public Task<Product?> UpdateProductAsync(Product product)
    {
        var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
        if (existingProduct == null)
        {
            return Task.FromResult<Product?>(null);
        }
        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.StockStatus = product.StockStatus;
        existingProduct.LastUpdated = DateTime.Now;
        return Task.FromResult<Product?>(existingProduct);
    }

    public Task<bool> DeleteProductAsync(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return Task.FromResult(false);
        }
        _products.Remove(product);
        return Task.FromResult(true);
    }

}