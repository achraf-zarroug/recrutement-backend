using backen_dotnet.Data;
using backen_dotnet.Dtos.Resume;
using backen_dotnet.Interfaces;
using backen_dotnet.Models.Resume;

namespace backen_dotnet.Repository
{
    public class CVRepository : ICVRepository
    {
        private readonly ApplicationDbContext _context;
        public CVRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<CV> CreateCvAsync(CreateCvDto cvDto)
        {
            // Mapping du DTO vers les entités
            var contactInfo = new ContactInfo
            {
                FirstName = cvDto.Contact.FirstName,
                LastName = cvDto.Contact.LastName,
                JobTitle = cvDto.Contact.JobTitle,
                Adress = cvDto.Contact.Adress,
                Ville = cvDto.Contact.Ville,
                ZipCode = cvDto.Contact.ZipCode,
                Phone = cvDto.Contact.Phone,
                Email = cvDto.Contact.Email
                
            };

            var educationInfo = new EducationInfo
            {
                SchoolName = cvDto.Education.SchoolName,
                SchoolLocation = cvDto.Education.SchoolLocation,
                Degree = cvDto.Education.Degree,
                FieldofStudy = cvDto.Education.FieldofStudy,
                StartMonth = cvDto.Education.StartMonth,
                StartYear = cvDto.Education.StartYear,
                EndMonth = cvDto.Education.EndMonth,

                EndYear = cvDto.Education.EndYear
            };

            var workExperienceInfo = new WorkExperienceInfo
            {
                Position = cvDto.WorkExperience.Position,
                CompanyName = cvDto.WorkExperience.CompanyName,
                StartMonth = cvDto.WorkExperience.StartMonth,
                StartYear = cvDto.WorkExperience.StartYear,
                EndMonth = cvDto.WorkExperience.EndMonth,

                EndYear = cvDto.WorkExperience.EndYear
            };

            var skillsInfos = cvDto.SkillsList.Select(skill => new Skills
            {
                Name = skill.Skill,
                // Vous pouvez ajouter d'autres propriétés si nécessaire
            }).ToList();

            // Création de l'entité CV
            var cv = new CV
            {
                Contact = contactInfo,
                Education = educationInfo,
                WorkExperience = workExperienceInfo,
                SkillsList = skillsInfos
            };

            // Ajout du CV à la base de données
            await _context.CVs.AddAsync(cv);
            await _context.SaveChangesAsync();
            return cv;
        }
        public async Task<ContactInfo> AddContactInfoAsync(ContactInfo contactInfo)
        {
            await _context.ContactInfos.AddAsync(contactInfo);
            await _context.SaveChangesAsync();
            return contactInfo;
        }
        public async Task<CV> AddCVAsync(CV cv)
        {
            await _context.CVs.AddAsync(cv);
            await _context.SaveChangesAsync();
            return cv;
        }

        public async Task<EducationInfo> AddEducationAsync(EducationInfo education)
        {
            await _context.EducationInfos.AddAsync(education);
            await _context.SaveChangesAsync();
            return education;
        }

        public async Task<WorkExperienceInfo> AddExperienceAsync(WorkExperienceInfo experience)
        {
            await _context.WorkExperienceInfos.AddAsync(experience);
            await _context.SaveChangesAsync();
            return experience;
        }

        public async Task<Skills> AddSkillsAsync(Skills skill)
        {
            await _context.Skills.AddAsync(skill);
            await _context.SaveChangesAsync();
            return skill;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();

        }
    }
}
