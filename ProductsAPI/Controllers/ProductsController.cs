using Microsoft.AspNetCore.Mvc;
using ProductsAPI.Repository;
using ProductsAPI.Models;

namespace ThAmCo.ProductsAPI.Controllers;

[Route("products")]
[ApiController]
public class ProductsController : ControllerBase
{
	private readonly IProductRepository _repo;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductRepository repo, ILogger<ProductsController> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    // GET: api/Products
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        IEnumerable<Product> products = await _repo.GetProductsAsync();
        return Ok(products);
    }

    // GET: api/Products/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _repo.GetProductAsync(id);

        if (product == null)
        {
            _logger.LogWarning($"Product with ID {id} not found.");
            return NotFound();
        }
        return Ok(product);
    }

    // POST: api/Products
    [HttpPost]
    public async Task<IActionResult> PostProduct([FromBody] Product product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var createdProduct = await _repo.AddProductAsync(product);
            if (createdProduct == null)
            {
                return BadRequest("Unable to add the product");
            }
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // PUT: api/Products/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(int id, [FromBody] Product product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (id != product.Id)
        {
            return BadRequest();
        }

        try
        {
            var updatedProduct = await _repo.UpdateProductAsync(product);
            if (updatedProduct == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }
            return Ok(updatedProduct);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // DELETE: api/Products/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            var product = await _repo.GetProductAsync(id);
            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            var success = await _repo.DeleteProductAsync(id);
            if (!success)
            {
                return BadRequest("Delete operation failed");
            }

            return Ok($"Product with ID {id} has been deleted.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}