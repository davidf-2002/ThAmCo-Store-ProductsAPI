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
builder.Services.AddControllers();
builder.Services.AddLogging();



// Validate JWT tokens issued by Auth server
var domain = builder.Configuration["Auth0:Domain"];
var audience = builder.Configuration["Auth0:Audience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>  // This configures the JWT Bearer authentication handler, where the client presents a bearer token.
{
    options.Authority = domain;  // This URL is used to obtain the public keys to validate the signature of the token.
    options.Audience = audience; // Ensures that the token is presented to the correct application

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


if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSingleton<IProductRepository, ProductRepositoryFake>();  // Singleton ensures that the state of the fake data persists across multiple requests
}
else 
{
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
}

// Configure DBContext with the DB connection string
builder.Services.AddDbContext<ProductContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("The connection string 'DefaultConnection' is not configured.");
    }
    // Retry pattern for resilience
    options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(6),
            errorNumbersToAdd: null
        )
    );
});



var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", context =>
{
    context.Response.Redirect("/products");
    return Task.CompletedTask;
});


app.Run();