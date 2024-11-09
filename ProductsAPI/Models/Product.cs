using System;
using System.ComponentModel.DataAnnotations;

namespace ProductsAPI.Models;

public class Product
{
    public int Id { get; set;}
    public string? Name { get; set; } 
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? StockStatus { get; set;}
    public DateTime LastUpdated { get; set; }

    // public int? SupplierId { get; set; }          // ID of the supplier
    // public Suppliers? Supplier { get; set; }      // Navigation property for the supplier
}