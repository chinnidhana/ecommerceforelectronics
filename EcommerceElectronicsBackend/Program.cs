// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
// using System.Text.Json.Serialization;
// using EcommerceElectronicsBackend.Data;
// using System.Text;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;

// using Microsoft.OpenApi.Models;
// using Microsoft.EntityFrameworkCore; // Added for EF Core
// // using YourNamespace; // Uncomment and replace with your actual namespace for ApplicationDbContext


// var builder = WebApplication.CreateBuilder(args);

// var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
// })
// .AddJwtBearer(options =>
// {
//     options.RequireHttpsMetadata = false;
//     options.SaveToken = true;
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuer           = true,
//         ValidateAudience         = true,
//         ValidateLifetime         = true,
//         ValidateIssuerSigningKey = true,
//         ValidIssuer              = builder.Configuration["Jwt:Issuer"],
//         ValidAudience            = builder.Configuration["Jwt:Audience"],
//         IssuerSigningKey         = new SymmetricSecurityKey(key)
//     };
// });



// // Add services to the container.
// builder.Services.AddControllers()
// .AddJsonOptions(options =>
//     {
//         options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
//         options.JsonSerializerOptions.WriteIndented = true;
//     });

// // Register the ApplicationDbContext with MySQL using Pomelo provider
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo
//     {
//         Title = "Ecommerce API",
//         Version = "v1",
//         Description = "API for managing ecommerce products",
//         Contact = new OpenApiContact
//         {
//             Name = "Support Team",
//             Email = "support@ecommerce.com",
//         }
//     });
// });

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseDeveloperExceptionPage();
//     app.UseSwagger();
    
//       app.UseSwaggerUI(c =>
//   {
//       c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecommerce API v1");
//       c.RoutePrefix = string.Empty;
//   });

// }

// app.UseHttpsRedirection();
// app.UseAuthentication();
// app.UseAuthorization();
// app.MapControllers();
// app.UseRouting();



// app.Run();

using System.Text;
using EcommerceElectronicsBackend.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using EcommerceElectronicsBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Read MySQL connection string
var mysqlConn = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Configure EF Core to use Pomelo MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(mysqlConn, ServerVersion.AutoDetect(mysqlConn))
);

builder.Services.AddScoped<ITokenService, TokenService>();
// 3. Read JWT settings (make sure your appsettings.json has these under "jwt")
var jwtSection = builder.Configuration.GetSection("jwt");
var jwtKey     = jwtSection["Key"];
var jwtIssuer  = jwtSection["Issuer"];
var jwtAudience= jwtSection["Audience"];
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!));

// 4. Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken            = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer           = true,
        ValidateAudience         = true,
        ValidateLifetime         = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer              = jwtIssuer,
        ValidAudience            = jwtAudience,
        IssuerSigningKey         = signingKey
    };
});

// 5. MVC + JSON options
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.WriteIndented     = true;
        opts.JsonSerializerOptions.ReferenceHandler  = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

// 6. Swagger (optional but handy)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "E-Commerce Electronics API",
        Version = "v1",
        Description = "Manage products, users & orders"
    });
});

var app = builder.Build();

// 7. Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Commerce Electronics API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

// **Routing must come before Auth & after MVC if you’re using endpoint routing**
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
