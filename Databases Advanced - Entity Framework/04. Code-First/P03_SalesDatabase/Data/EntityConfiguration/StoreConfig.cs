using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_SalesDatabase.Data.Models;

namespace P03_SalesDatabase.Data.EntityConfiguration
{
    public class StoreConfig : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.HasKey(s => s.StoreId);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(80)
                .IsUnicode();

            builder.HasMany(s => s.Sales)
                .WithOne(s => s.Store)
                .HasForeignKey(s => s.StoreId);
        }
    }
}
