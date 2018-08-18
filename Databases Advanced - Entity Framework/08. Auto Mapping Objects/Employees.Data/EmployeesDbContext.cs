namespace Employees.Data
{
    using Microsoft.EntityFrameworkCore;

    using Models;

    public class EmployeesDbContext : DbContext
    {
        public EmployeesDbContext()
        { }

        public EmployeesDbContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasOne(e => e.Manager)
                    .WithMany(m => m.ManagerEmployees)
                    .HasForeignKey(e => e.ManagerId);
            });
        }
    }
}
