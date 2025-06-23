using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backen_dotnet.Models
{
    public class Candidature
{
    public string AppUserId { get; set; }
    public int OffreId { get; set; }
    public string LettreMotivationName { get; set; }
    public string CvFileName { get; set; }
    public double Score { get; set; }
    public string Status { get; set; }
    public string Decision { get; set; }
    public DateTime AplliedDate { get; set; }
    public virtual AppUser AppUser { get; set; }
    public virtual Offre Offre { get; set; }
}
}
