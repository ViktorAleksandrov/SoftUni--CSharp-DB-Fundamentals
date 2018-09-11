namespace PetClinic.Data
{
    using Microsoft.EntityFrameworkCore;

    using PetClinic.Models;

    public class PetClinicContext : DbContext
    {
        public PetClinicContext() { }

        public PetClinicContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Animal> Animals { get; set; }

        public DbSet<AnimalAid> AnimalAids { get; set; }

        public DbSet<Passport> Passports { get; set; }

        public DbSet<Procedure> Procedures { get; set; }

        public DbSet<ProcedureAnimalAid> ProceduresAnimalAids { get; set; }

        public DbSet<Vet> Vets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AnimalAid>()
                .HasIndex(aa => aa.Name)
                .IsUnique();

            builder.Entity<ProcedureAnimalAid>()
                .HasKey(paa => new { paa.ProcedureId, paa.AnimalAidId });

            builder.Entity<Vet>()
                .HasIndex(v => v.PhoneNumber)
                .IsUnique();
        }
    }
}
