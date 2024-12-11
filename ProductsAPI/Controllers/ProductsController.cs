using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsAPI.Models;
using ProductsAPI.Repository;
using Microsoft.EntityFrameworkCore;

namespace ProductsAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductsController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    // GET: api/Products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return Ok(await _productRepository.GetProductsAsync());
    }

    // GET: api/Products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _productRepository.GetProductAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return product;
    }

    // POST: api/Products
    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(Product product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var createdProduct = await _productRepository.AddProductAsync(product);
        return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
    }

    // PUT: api/Products/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(int id, Product product)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _productRepository.UpdateProductAsync(product);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await _productRepository.GetProductAsync(id) == null)
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // // DELETE: api/Products/5
    // [HttpDelete("{id}")]
    // public async Task<IActionResult> DeleteProduct(int id)
    // {
    //     var product = await _productRepository.GetProductAsync(id);
    //     if (product == null)
    //     {
    //         return NotFound();
    //     }

    //     await _productRepository.DeleteProductAsync(id);
    //     return NoContent();
    // }
}
