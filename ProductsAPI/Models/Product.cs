using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProductsAPI.Models;

public class Product
{
    public Product()
    {
        LastUpdated = DateTime.Now;
        ProductSuppliers = new List<ProductSupplier>();
    }
    
    [Key]
    public int Id { get; set;}

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime LastUpdated { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [JsonIgnore]
    public Category? Category { get; set; } = null!;

    public int StockLevel { get; set; }

    [JsonIgnore]
    public List<ProductSupplier> ProductSuppliers { get; set; }

    public string StockStatus 
    {
        get
        {
            if (StockLevel <= 0) return "Out of Stock";
            if (StockLevel <= 5) return "Low Stock";
            return "In Stock";
        }
    }
}