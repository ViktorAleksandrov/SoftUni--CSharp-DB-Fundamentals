using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data.EntityConfiguration
{
    public class PatientConfig : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(p => p.PatientId);

            builder.Property(p => p.FirstName)
                .HasMaxLength(50)
                .IsUnicode();

            builder.Property(p => p.LastName)
                .HasMaxLength(50)
                .IsUnicode();

            builder.Property(p => p.Address)
                .HasMaxLength(250)
                .IsUnicode();

            builder.Property(p => p.Email)
                .HasMaxLength(80)
                .IsUnicode(false);

            builder.HasMany(p => p.Prescriptions)
                .WithOne(p => p.Patient)
                .HasForeignKey(p => p.PatientId);

            builder.HasMany(p => p.Visitations)
                .WithOne(p => p.Patient)
                .HasForeignKey(p => p.PatientId);

            builder.HasMany(p => p.Diagnoses)
                .WithOne(p => p.Patient)
                .HasForeignKey(p => p.PatientId);
        }
    }
}
