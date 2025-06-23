using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace backen_dotnet.Models
{
     public class AppUser : IdentityUser
 {
     public virtual List<Candidature> Candidatures { get; set; }
     public virtual List<Interview> Interviews { get; set; }

     public DateTime DateCreated { get; set; }


 }
}
