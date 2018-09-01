using CarDealer.Models;
using Microsoft.EntityFrameworkCore;

namespace CarDealer.Data
{
    public class CarDealerContext : DbContext
    {
        public CarDealerContext()
        {
        }

        public CarDealerContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Part> Parts { get; set; }

        public DbSet<PartCar> PartCars { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ServerConfig.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PartCar>(entity =>
            {
                entity.HasKey(pc => new { pc.PartId, pc.CarId });

                entity.HasOne(pc => pc.Part)
                    .WithMany(p => p.PartCars)
                    .HasForeignKey(pc => pc.PartId);

                entity.HasOne(pc => pc.Car)
                    .WithMany(c => c.PartCars)
                    .HasForeignKey(pc => pc.CarId);
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasOne(s => s.Car)
                    .WithMany(c => c.Sales)
                    .HasForeignKey(s => s.CarId);

                entity.HasOne(s => s.Customer)
                    .WithMany(c => c.BoughtCars)
                    .HasForeignKey(s => s.CustomerId);
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasMany(s => s.Parts)
                    .WithOne(p => p.Supplier)
                    .HasForeignKey(p => p.SupplierId);
            });
        }
    }
}
