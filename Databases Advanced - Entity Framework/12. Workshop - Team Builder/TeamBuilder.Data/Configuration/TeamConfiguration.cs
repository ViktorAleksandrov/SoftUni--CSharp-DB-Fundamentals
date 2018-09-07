using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamBuilder.Models;

namespace TeamBuilder.Data.Configuration
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasIndex(t => t.Name).IsUnique();

            builder.HasMany(t => t.SentInvitations)
                .WithOne(i => i.Team)
                .HasForeignKey(i => i.TeamId);
        }
    }
}
