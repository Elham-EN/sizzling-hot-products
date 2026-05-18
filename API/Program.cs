using System.Text.Json;
using API.Models;
using API.Services;

var builder = WebApplication.CreateBuilder(args);

// Load orders and products from the solution-level inputs/ folder as required by the challenge
var inputsPath = Path.Combine(builder.Environment.ContentRootPath, "..", "inputs");

var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

var orders = JsonSerializer.Deserialize<List<Order>>(
    File.ReadAllText(Path.Combine(inputsPath, "orders.json")), jsonOptions) ?? [];

var products = JsonSerializer.Deserialize<List<Product>>(
    File.ReadAllText(Path.Combine(inputsPath, "products.json")), jsonOptions) ?? [];

// Register the service as a singleton — data is read once at startup
builder.Services.AddSingleton<ISizzlingHotProductService>(
    new SizzlingHotProductService(orders, products));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Allow the React frontend to call the API during local development
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");
app.MapControllers();

app.Run();
