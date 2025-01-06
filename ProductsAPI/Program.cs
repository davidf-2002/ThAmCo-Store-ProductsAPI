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

builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Configure DbContext using the connection string, with Retry pattern
builder.Services.AddDbContext<ProductContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var dbPath = System.IO.Path.Join(path, "products.db");
        options.UseSqlite($"Data source={dbPath}");
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
    }
    else
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("The connection string 'DefaultConnection' is not configured.");
        }
        options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(6),
                errorNumbersToAdd: null
            )
        );
    }
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

// This creates a new scope to perform data seeding 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    if (app.Environment.IsDevelopment())
    {
        var context = services.GetRequiredService<ProductContext>();
        context.Database.Migrate();
        try
        {
            ProductInitialiser.Initialise(context);
        }
        catch (Exception e)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogDebug("Inserting test data failed.");
        }
    }
}

app.Run();