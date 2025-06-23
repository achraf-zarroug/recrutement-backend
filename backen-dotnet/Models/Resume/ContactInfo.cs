using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backen_dotnet.Models.Resume
{
    public class ContactInfo
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? JobTitle { get; set; }

        public string? Adress { get; set; }
        public string? ZipCode { get; set; }
        public string? Ville { get; set; }
        public string? Pays { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Bio { get; set; }
        public int CvId { get; set; }
        public virtual CV cv { get; set; }

    }
}
