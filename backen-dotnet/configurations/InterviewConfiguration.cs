using backen_dotnet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backen_dotnet.configurations
{
    public class InterviewConfiguration : IEntityTypeConfiguration<Interview>
    {
        public void Configure(EntityTypeBuilder<Interview> builder)
        {
            builder.HasKey(t => t.InterviewId); // Si InterviewId est votre clé primaire

            // Relation entre Interview et Candidat
            builder.HasOne(i => i.AppUser)  // Si Interview a une relation avec Candidat
                   .WithMany(c => c.Interviews) // Cela implique que Candidat a une collection d'entretiens
                   .HasForeignKey(i => i.AppUserId); // Clé étrangère vers Candidat

            // Relation entre Interview et Offre
            builder.HasOne(i => i.Offre)  // Si Interview est lié à une Offre
                   .WithMany(o => o.Interviews) // Cela implique qu'Offre a une collection d'entretiens
                   .HasForeignKey(i => i.OffreId); // Clé étrangère vers Offre

        // Configuration pour la date et l'heure de l'entretien
        builder.Property(i => i.AppointmentDateTime)
               .IsRequired();
        }
    }
}
