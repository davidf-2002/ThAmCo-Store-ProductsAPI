using System.ComponentModel.DataAnnotations;

namespace ProductsAPI.Models;

public class Category
{
    public Category()
    {
        Products = new List<Product>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } =string.Empty;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
