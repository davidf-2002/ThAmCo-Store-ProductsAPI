using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsAPI.Models;

public class Product
{
    public Product()
    {
        LastUpdated = DateTime.Now;
        ProductSuppliers = new List<ProductSupplier>();
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set;}

    [Required]
    [StringLength(100)]
    public string? Name { get; set; } 

    [Required]
    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required]
    [StringLength(50)]
    public string? StockStatus { get; set;}

    [Column(TypeName = "datetime2")]
    public DateTime LastUpdated { get; set; }

    [Required]
    public String Category { get; set; }

    public int StockLevel { get; set; }

    public List<ProductSupplier> ProductSuppliers { get; set; }
}