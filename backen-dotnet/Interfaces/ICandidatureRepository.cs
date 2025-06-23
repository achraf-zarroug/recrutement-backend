using backen_dotnet.Models;

namespace backen_dotnet.Interfaces
{
   public interface ICandidatureRepository
{
    Task<List<Offre>> GetUserOffre(AppUser user);
    Task<List<Candidature>> GetAllAsync();
    Task<List<Candidature>> GetCandidaturesByUser(AppUser user);
    Task<bool> UpdateDecisionAsync(string appUserId, int offreId, string decision);
    Task<List<Candidature>> GetCandidaturesByOffer(Offre offre);
    Task<Candidature> GetCandidatureById(string appUserId, int offreId);

    Task<Candidature> CreateAsync(Candidature candidature);
    Task<Candidature?> DeleteAsync(string appUserId, int offreId);
}
}
