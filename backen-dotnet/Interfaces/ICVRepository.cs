using backen_dotnet.Dtos.Resume;
using backen_dotnet.Models.Resume;

namespace backen_dotnet.Interfaces
{
    public interface ICVRepository
 {
     Task<ContactInfo> AddContactInfoAsync(ContactInfo contactInfo);
     Task<WorkExperienceInfo> AddExperienceAsync(WorkExperienceInfo experience);
     Task<EducationInfo> AddEducationAsync(EducationInfo education);
     Task<Skills> AddSkillsAsync(Skills skill);
     Task SaveChangesAsync();
     Task<CV> AddCVAsync(CV cv);
     Task<CV> CreateCvAsync(CreateCvDto cvDto);

 }
}
