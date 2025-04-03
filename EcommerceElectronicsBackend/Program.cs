using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;
using EcommerceElectronicsBackend.Data;

using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore; // Added for EF Core
// using YourNamespace; // Uncomment and replace with your actual namespace for ApplicationDbContext


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
.AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Register the ApplicationDbContext with MySQL using Pomelo provider
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Ecommerce API",
        Version = "v1",
        Description = "API for managing ecommerce products",
        Contact = new OpenApiContact
        {
            Name = "Support Team",
            Email = "support@ecommerce.com",
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    
      app.UseSwaggerUI(c =>
  {
      c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecommerce API v1");
      c.RoutePrefix = string.Empty;
  });

}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseRouting();



app.Run();