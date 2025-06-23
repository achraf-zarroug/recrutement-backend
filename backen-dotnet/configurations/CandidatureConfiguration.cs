using backen_dotnet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Net.Sockets;

namespace backen_dotnet.configurations
{
    public class CandidatureConfiguration : IEntityTypeConfiguration<Candidature>
    {
        public void Configure(EntityTypeBuilder<Candidature> builder)
        {
            builder.HasKey(t => new { t.OffreId, t.AppUserId });//Clé primaire composée 

            builder.HasOne(t => t.AppUser)
                    .WithMany(f => f.Candidatures)
                    .HasForeignKey(t => t.AppUserId);
            
            builder.HasOne(t => t.Offre)
                   .WithMany(p => p.Candidatures)
                   .HasForeignKey(t => t.OffreId);
        }
    }
}
