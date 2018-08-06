using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data.EntityConfiguration
{
    public class DoctorConfig : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(d => d.DoctorId);

            builder.Property(d => d.Name)
                .HasMaxLength(100)
                .IsUnicode();

            builder.Property(d => d.Specialty)
                .HasMaxLength(100)
                .IsUnicode();

            builder.HasMany(d => d.Visitations)
                .WithOne(d => d.Doctor)
                .HasForeignKey(d => d.DoctorId);
        }
    }
}
