using System.ComponentModel.DataAnnotations;

namespace ProductsAPI.Models;

public class ProductSupplier
{
    public ProductSupplier()
    {
    }
    
    [Key]
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    [Required]
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; }
}