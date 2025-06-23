using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backen_dotnet.Models.Resume
{
    public class Skills
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CvId { get; set; }
        public virtual CV cv { get; set; }

    }
}
