using Microsoft.EntityFrameworkCore;
using EcommerceElectronicsBackend.Models;

namespace EcommerceElectronicsBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            // Configure Categories entity to match your SQL table
            modelBuilder.Entity<Category>(entity =>
            {
                // Use the exact table name as in your SQL (note case sensitivity if applicable)
                entity.ToTable("categories");

                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CategoryId)
                    .HasColumnName("category_id");

                entity.Property(e => e.CategoryName)
                    .HasColumnName("category_name")
                    .HasColumnType("varchar(100)")
                    .IsRequired();
            });

            // Configure Products entity to match your SQL table
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");

                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id");

                entity.Property(e => e.ProductName)
                    .HasColumnName("product_name")
                    .HasColumnType("varchar(100)")
                    .IsRequired();

                entity.Property(e => e.CategoryId)
                    .HasColumnName("category_id")
                    .IsRequired();

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.Property(e => e.StockQuantity)
                    .HasColumnName("stock_quantity")
                    .IsRequired();

                // Map the created_at column
                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Map the updated_at column
                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Define the foreign key relationship with Categories
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("users");
                e.HasKey(u => u.UserId);
                e.Property(u => u.UserId)        .HasColumnName("user_id");
                e.Property(u => u.Username)      .HasColumnName("username")     .HasMaxLength(50)  .IsRequired();
                e.Property(u => u.Email)         .HasColumnName("email")        .HasMaxLength(100) .IsRequired();
                e.Property(u => u.PasswordHash)  .HasColumnName("password_hash").HasMaxLength(255) .IsRequired();
                e.Property(u => u.Role)          .HasColumnName("role")         .HasMaxLength(20)  .IsRequired();
            });



        }
    }
}
