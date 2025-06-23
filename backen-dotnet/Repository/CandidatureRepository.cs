using backen_dotnet.Data;
using backen_dotnet.Interfaces;
using backen_dotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace backen_dotnet.Repository
{
    public class CandidatureRepository : ICandidatureRepository
{
    private readonly ApplicationDbContext _context;
    public CandidatureRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Candidature> CreateAsync(Candidature candidature)
    {
        await _context.Candidatures.AddAsync(candidature);
        await _context.SaveChangesAsync();  
        return candidature;
    }
    public async Task<List<Candidature>> GetAllAsync()
    {
        return await _context.Candidatures.ToListAsync();
    }

    public async Task<List<Candidature>> GetCandidaturesByOffer(Offre offre)
    {
        var candidatures = await _context.Candidatures
            .Include(c => c.AppUser)
            .Where(c => c.OffreId == offre.Id)
            .Select(c => new Candidature
            {
                AppUserId = c.AppUserId,
                OffreId = c.OffreId,
                LettreMotivationName = c.LettreMotivationName,
                CvFileName = c.CvFileName,
                Score = c.Score,
                Decision = c.Decision,
                AplliedDate = c.AplliedDate,
                AppUser = new AppUser
                {
                    UserName = c.AppUser.UserName,
                    Email = c.AppUser.Email,
                    PhoneNumber = c.AppUser.PhoneNumber,
                }
            })
             .ToListAsync();

        return candidatures;
    }
    public async Task<bool> UpdateDecisionAsync(string appUserId, int offreId, string decision)
    {
     //   var candidature = await _context.Candidatures.FindAsync(candidatureId);
        var candidature = await this.GetCandidatureById(appUserId, offreId);

        if (candidature == null)
        {
            return false;
        }

        candidature.Decision = decision;
        _context.Candidatures.Update(candidature);
        await _context.SaveChangesAsync();

        return true;
    }
    public async Task<List<Candidature>> GetCandidaturesByUser(AppUser user)
    {
        // return await _context.Candidatures
        // .Where(c => c.AppUserId == user.Id)
        //.ToListAsync();
        var candidatures = await _context.Candidatures
   .Include(c => c.Offre) 
   .Where(c => c.AppUserId == user.Id)
   .Select(c => new Candidature
   {
       AppUserId = c.AppUserId,
       OffreId = c.OffreId,
       LettreMotivationName = c.LettreMotivationName,
       CvFileName = c.CvFileName,
       Offre = new Offre
       {
           Titre = c.Offre.Titre ,
           Description = c.Offre.Description,
           Qualifications = c.Offre.Qualifications,
           Responsibilities = c.Offre.Responsibilities,
           DateDebut = c.Offre.DateDebut,
           PostedDate = c.Offre.PostedDate,
           EstStage = c.Offre.EstStage,
           Departement = c.Offre.Departement,
           Lieu = c.Offre.Lieu,
           ContractType = c.Offre.ContractType,
           ContractDuration = c.Offre.ContractDuration,
           Gender = c.Offre.Gender,
           NiveauEtudesRequis = c.Offre.NiveauEtudesRequis,
           EstActive = c.Offre.EstActive,
                                 
       }
   })
   .ToListAsync();
        return candidatures;

    }
    public async Task<Candidature> GetCandidatureById(string appUserId, int offreId)
    {
        return await _context.Candidatures
           .Include(c => c.AppUser)
           .Include(c => c.Offre)
           .FirstOrDefaultAsync(c => c.AppUserId == appUserId && c.OffreId == offreId);
    }
    public async Task<List<Offre>> GetUserOffre(AppUser user)
    {
        return await _context.Candidatures.Where(u => u.AppUserId == user.Id)
            .Select(offre => new Offre
            {
                Id = offre.OffreId,
                Titre = offre.Offre.Titre,
                Description = offre.Offre.Description,
                Qualifications = offre.Offre.Qualifications,
                Responsibilities = offre.Offre.Responsibilities,
                DateDebut = offre.Offre.DateDebut,
                PostedDate = offre.Offre.PostedDate,
                EstStage = offre.Offre.EstStage,
                Departement = offre.Offre.Departement,
                Lieu = offre.Offre.Lieu,
                ContractType = offre.Offre.ContractType,
                ContractDuration = offre.Offre.ContractDuration,
                Gender = offre.Offre.Gender,
                NiveauEtudesRequis = offre.Offre.NiveauEtudesRequis,
                EstActive = offre.Offre.EstActive,
            }).ToListAsync();
    }
    public async Task<Candidature?> DeleteAsync(string appUserId, int offreId)
    {
        var candidatureModel = await _context.Candidatures.FirstOrDefaultAsync(c => c.AppUserId == appUserId && c.OffreId == offreId);
        if (candidatureModel == null)
        {
            return null;
        }
        _context.Candidatures.Remove(candidatureModel);
        await _context.SaveChangesAsync();
        return candidatureModel;
    }
}
}
