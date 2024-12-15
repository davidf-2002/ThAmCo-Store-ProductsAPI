using ProductsAPI.Repository;
using ProductsAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

// Configure DbContext using the connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ProductContext>(options =>
    options.UseSqlServer(connectionString));
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("The connection string 'DefaultConnection' is not configured.");
}

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSingleton<IProductRepository, ProductRepositoryFake>();  // Using Singleton ensures that the state of the fake data persists across multiple requests
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

app.UseRouting();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();


app.Run();