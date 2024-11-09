using System;
using ProductsAPI.Models;

namespace ProductsAPI.Repository;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<Product?> GetProductAsync(int id);
    Task<Product> AddProductAsync(Product product);
    Task<Product> UpdateProductAsync(Product prodcut);
    void DeleteProductAsync(int id);
}