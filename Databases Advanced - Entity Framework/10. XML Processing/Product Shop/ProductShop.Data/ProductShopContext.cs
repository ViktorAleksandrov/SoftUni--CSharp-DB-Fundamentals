using Microsoft.EntityFrameworkCore;
using ProductShop.Models;

namespace ProductShop.Data
{
    public class ProductShopContext : DbContext
    {
        public ProductShopContext()
        {
        }

        public ProductShopContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<CategoryProduct> CategoryProducts { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ServerConfig.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryProduct>()
                .HasKey(c => new { c.CategoryId, c.ProductId });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasMany(u => u.BoughtProducts)
                    .WithOne(p => p.Buyer)
                    .HasForeignKey(p => p.BuyerId);

                entity.HasMany(u => u.SoldProducts)
                    .WithOne(p => p.Seller)
                    .HasForeignKey(p => p.SellerId);
            });
        }
    }
}
