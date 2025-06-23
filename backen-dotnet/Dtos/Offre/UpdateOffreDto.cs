namespace backen_dotnet.Dtos.Offre
{
     public class UpdateOffreDto
 {
     public string Titre { get; set; }
     public string Description { get; set; }
     public string Qualifications { get; set; }
     public string Responsibilities { get; set; }
     public DateTime DateDebut { get; set; }
     public DateTime PostedDate { get; set; }
     public List<string> RequiredSkills { get; set; }
     public List<string> RequiredLanguages { get; set; }
     public List<string> Benefits { get; set; }
     public bool EstStage { get; set; }
     public string Departement { get; set; }
     public string Lieu { get; set; }
     public string ContractType { get; set; }
     public string? ContractDuration { get; set; }

     public string Gender { get; set; }
     public string NiveauEtudesRequis { get; set; }
     public bool EstActive { get; set; }
 }
}
