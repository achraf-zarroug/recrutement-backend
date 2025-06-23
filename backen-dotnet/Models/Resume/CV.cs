using System.ComponentModel.DataAnnotations;

namespace backen_dotnet.Models.Resume
{
    public class CV
    {

        public int Id { get; set; }
        public virtual ContactInfo Contact { get; set; }
        public virtual EducationInfo Education { get; set; }
        public virtual WorkExperienceInfo WorkExperience { get; set; }

        public virtual List<Skills> SkillsList { get; set; }

    }
}
