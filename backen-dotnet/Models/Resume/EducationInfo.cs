using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backen_dotnet.Models.Resume
{
    public class EducationInfo
    {

        public int Id { get; set; }
        public string SchoolName { get; set; }
        public string SchoolLocation { get; set; }
        public DateTime StartMonth { get; set; }
        public DateTime StartYear { get; set; }
        public DateTime EndMonth { get; set; }
        public DateTime EndYear { get; set; }
        public string Degree { get; set; }
        public string FieldofStudy { get; set; }
        public int CvId { get; set; }
        public virtual CV cv { get; set; }


    }
}
