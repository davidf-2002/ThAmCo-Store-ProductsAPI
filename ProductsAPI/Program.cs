using ProductsAPI.Repository;
using ProductsAPI.Models;
using ProductsAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// // Configure DbContext using the connection string
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// builder.Services.AddDbContext<ProductContext>(options =>
//     options.UseSqlServer(connectionString));
// if (string.IsNullOrEmpty(connectionString))
// {
//     throw new InvalidOperationException("The connection string 'DefaultConnection' is not configured.");
// }

builder.Services.AddDbContext<ProductContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var dbPath = Path.Join(path, "comments.db");
        options.UseSqlite($"Data Source={dbPath}");
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
    }
    else
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        options.UseSqlServer(connectionString);
    }
});


if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSingleton<IProductRepository, ProductRepositoryFake>(); // Using Singleton ensures that the state of the fake data persists across multiple requests
}
else 
{
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// GET: api/Products
app.MapGet("/products", async (IProductRepository repo) =>
{
    var products = await repo.GetProductsAsync();
    return Results.Ok(products);
})
.WithName("GetProducts")
.WithOpenApi();

// GET: api/Products/5
app.MapGet("/products/{id}", async (int id, IProductRepository repo) =>
{
    var product = await repo.GetProductAsync(id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
})
.WithName("GetProductById")
.WithOpenApi();

// POST: api/Products
app.MapPost("/products", async (Product product, IProductRepository repo) =>
{
    try
    {
        var createdProduct = await repo.AddProductAsync(product);
        if (createdProduct is null)
        {
            return Results.Problem("Product could not be created");
        }
        return Results.Created($"/products/{createdProduct.Id}", createdProduct);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.WithName("AddProduct")
.WithOpenApi();

// PUT: api/Products/5
app.MapPut("/products/{id}", async (int id, Product product, IProductRepository repo) =>
{
    var existingProduct = await repo.GetProductAsync(id);       // Check if the product exists
    if (existingProduct is null)
    {
        return Results.NotFound();
    }
    try
    {
        product.Id = id; // Ensure the product ID is set correctly.
        var updatedProduct = await repo.UpdateProductAsync(product);
        if (updatedProduct is null)
        {
            return Results.Problem("Product could not be updated");
        }
        return Results.Ok(updatedProduct);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.WithName("UpdateProduct")
.WithOpenApi();

// DELETE: api/Products/5
app.MapDelete("/products/{id}", async (int id, IProductRepository repo) =>
{
    try
    {
        var success = await repo.DeleteProductAsync(id);
        if (success)
        {
            return Results.Ok($"Product with ID {id} deleted successfully.");
        }
        else
        {
            return Results.NotFound();
        }
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.WithName("DeleteProduct")
.WithOpenApi();

app.Run();