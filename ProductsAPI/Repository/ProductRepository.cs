using System;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Models;
using ProductsAPI.Data;

namespace ProductsAPI.Repository;

public class ProductRepository : IProductRepository
{
    private readonly ProductContext _context;

    public ProductRepository(ProductContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        var products = await _context.Products
        .Include(p => p.ProductSuppliers)
        .ThenInclude(ps => ps.Supplier)
        .ToListAsync();        
        return await Task.FromResult(products);
    }

    public async Task<Product?> GetProductAsync(int id)
    {
        var product = await _context.Products
        .Include(p => p.ProductSuppliers)
        .ThenInclude(ps => ps.Supplier)
        .FirstOrDefaultAsync(p => p.Id == id);
        return product;
    }

    public async Task<Product?> AddProductAsync(Product product)
    {
        var category = await _context.Categories.FindAsync(product.CategoryId);
        if (category == null)
        {
            throw new ArgumentException("Category does not exist");
        }
        product.Category = category;
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> UpdateProductAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

}