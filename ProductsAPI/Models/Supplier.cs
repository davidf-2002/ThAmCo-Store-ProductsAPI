using System.ComponentModel.DataAnnotations;

namespace ProductsAPI.Models;

public class Supplier
{
    public Supplier()
    {
        ProductSuppliers = new List<ProductSupplier>();
    }

    [Key]
    public int Id { get; set; }

    [Required] 
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(100)]
    public string Email { get; set; }

    public List<ProductSupplier> ProductSuppliers { get; set; }
}