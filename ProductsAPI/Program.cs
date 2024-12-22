using ProductsAPI.Repository;
using ProductsAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();


// Provision Auth server using Auth0
var domain = "https://dev-4nmkq13nhzjzkjvm.eu.auth0.com/";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>  // This configures the JWT Bearer authentication handler, where the client presents a bearer token.
{
    options.Authority = builder.Configuration["Auth0:Domain"];  // This URL is used to obtain the public keys to validate the signature of the token.
    options.Audience = builder.Configuration["Auth0:Audience"]; // Ensures that the token is presented to the correct application

    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = ClaimTypes.NameIdentifier
    };
});

// Add policies for the scopes
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("write:products", policy => policy.Requirements.Add(new
    HasScopeRequirement("write:products", domain)));
});
builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();


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
    //builder.Services.AddSingleton<IProductRepository, ProductRepositoryFake>();  // Using Singleton ensures that the state of the fake data persists across multiple requests
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
}
else 
{
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
}


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = ""; 
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();