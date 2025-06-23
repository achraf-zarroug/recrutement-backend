using backen_dotnet.configurations;
using backen_dotnet.Models;
using backen_dotnet.Models.Resume;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace backen_dotnet.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {

        }
        public virtual DbSet<Offre> Offres { get; set; }
        public virtual DbSet<Candidature> Candidatures { get; set; }
        public virtual DbSet<Interview> Interviews { get; set; }

        public virtual DbSet<EducationInfo> EducationInfos { get; set; }
        public virtual DbSet<ContactInfo> ContactInfos { get; set; }
        public virtual DbSet<CV> CVs { get; set; }
        public virtual DbSet<WorkExperienceInfo> WorkExperienceInfos { get; set; }
        public virtual DbSet<Skills> Skills { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
           

            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new CandidatureConfiguration());
            builder.ApplyConfiguration(new InterviewConfiguration());

            //builder.Entity<Candidature>(x => x.HasKey(c =>
            //new { c.AppUserId, c.OffreId }));


            //builder.Entity<Candidature>()
            //    .HasOne(u => u.Offre)
            //    .WithMany(u => u.Candidatures)
            //    .HasForeignKey(c => c.OffreId);
            builder.Entity<AppUser>()
               .Property(u => u.DateCreated)
               .HasDefaultValueSql("GETDATE()");

           

        }

    }
}