using backen_dotnet.Models;
using backen_dotnet.Models.Resume;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backen_dotnet.configurations
{
    public class CVConfiguration : IEntityTypeConfiguration<CV>
    {
        public void Configure(EntityTypeBuilder<CV> builder)
        {
            builder
                   .HasOne(c => c.Contact)
                   .WithOne(ci => ci.cv)
                   .HasForeignKey<ContactInfo>(ci => ci.CvId);

            builder
                .HasOne(c => c.Education)
                .WithOne(e => e.cv)
                .HasForeignKey<EducationInfo>(e => e.CvId);

            builder
                .HasOne(c => c.WorkExperience)
                .WithOne(w => w.cv)
                .HasForeignKey<WorkExperienceInfo>(w => w.CvId);

            builder
                .HasMany(c => c.SkillsList)
                .WithOne(s => s.cv)
                .HasForeignKey(s => s.CvId);
        }
    }
}
