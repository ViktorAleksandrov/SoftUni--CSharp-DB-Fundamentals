using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data.EntityConfiguration
{
    public class ResourceConfig : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.Property(c => c.Name)
                   .HasMaxLength(50)
                   .IsUnicode()
                   .IsRequired();

            builder.Property(c => c.Url)
                .IsUnicode(false)
                .IsRequired();
        }
    }
}
