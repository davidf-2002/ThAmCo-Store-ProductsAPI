using ProductsAPI.Models;

public class Suppliers
{
    public int Id { get; set; }
    public string? Name {get; set; }
    public string? ContactInformation { get; set; }

    public List<Product> Products { get; set; } = new List<Product>();
}