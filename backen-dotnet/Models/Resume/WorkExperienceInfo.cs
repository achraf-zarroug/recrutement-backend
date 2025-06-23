using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backen_dotnet.Models.Resume
{
    public class WorkExperienceInfo
    {

        public int Id { get; set; }
        public string Position { get; set; }
        public string CompanyName { get; set; }
        public DateTime StartMonth { get; set; }
        public DateTime StartYear { get; set; }
        public DateTime EndMonth { get; set; }
        public DateTime EndYear { get; set; }
        public int CvId { get; set; }

        public virtual CV cv { get; set; }

    }
}
