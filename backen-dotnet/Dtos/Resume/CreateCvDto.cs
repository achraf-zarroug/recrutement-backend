using backen_dotnet.Models.Resume;

namespace backen_dotnet.Dtos.Resume
{
    public class CreateCvDto
    {
        public virtual ContactInfoDto Contact { get; set; }
        public virtual EducationInfoDto Education { get; set; }
        public virtual WorkExperienceInfoDto WorkExperience { get; set; }

        public virtual List<SkillsDto> SkillsList { get; set; }
    }
}
