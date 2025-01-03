using System;
using ProductsAPI.Models;

namespace ProductsAPI.Repository;

public class ProductRepositoryFake : IProductRepository
{
    private readonly List<Product> _products = new List<Product>
    {
        new Product { Id = 1, Name = "T-shirt", Description = "Jack & Jones", Price = 11.50m, CategoryId = 1, StockLevel = 3, LastUpdated = new DateTime(2024, 11, 07)},
        new Product { Id = 2, Name = "Jeans", Description = "Armani", Price = 30.00m, CategoryId = 1, StockLevel = 0, LastUpdated = new DateTime(2024, 11, 07)},
        new Product { Id = 3, Name = "Hoody", Description = "Boss", Price = 20.99m, CategoryId = 1, StockLevel = 7, LastUpdated = new DateTime(2024, 11, 07)}    
    };

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        var products = _products.AsEnumerable();
        return await Task.FromResult(products);
    }

    public async Task<Product?> GetProductAsync(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        return await Task.FromResult(product);
    }

    public async Task<Product?> AddProductAsync(Product product)
    {
        int newId = _products.Max(p => p.Id) + 1; 
        product.Id = newId;
        _products.Add(product);
        return await Task.FromResult<Product?>(product);
    }

    public async Task<Product?> UpdateProductAsync(Product product)
    {
        var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
        if (existingProduct == null)
        {
            return await Task.FromResult<Product?>(null);
        }
        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.StockLevel = product.StockLevel;
        existingProduct.LastUpdated = DateTime.Now;
        return await Task.FromResult<Product?>(existingProduct);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return await Task.FromResult(false);
        }
        _products.Remove(product);
        return await Task.FromResult(true);
    }

}