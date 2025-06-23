using backen_dotnet.Data;
using backen_dotnet.Dtos.Offre;
using backen_dotnet.Interfaces;
using backen_dotnet.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace backen_dotnet.Repository
{
    public class OffreRepository : IOffreRepository
{
    private readonly ApplicationDbContext _context;

    public OffreRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Offre> CreateAsync(Offre offreModel)
    {
        await _context.Offres.AddAsync(offreModel);
        await _context.SaveChangesAsync();
        return offreModel;
    }

    public  async Task<Offre?> DeleteAsync(int id)
    {
       var offreModel = await _context.Offres.FirstOrDefaultAsync(x => x.Id == id);
        if(offreModel == null)
        {
            return null;
        }
        _context.Offres.Remove(offreModel);
        await _context.SaveChangesAsync();
        return offreModel;
    }

    public async Task<List<Offre>> GetAllAsync()
    {
       return await _context.Offres.Include(o => o.Candidatures).ToListAsync();
    }

    public async Task<Offre?> GetByIdAsync(int id)
    {
        return await _context.Offres.FindAsync(id);
    }

    public async Task<Offre?> GetByTitreAsync(string titre)
    {
        return await _context.Offres.FirstOrDefaultAsync(t => t.Titre == titre);    
    }

    public async Task<Offre?> UpdateAsync(int id, UpdateOffreDto offreDto)
    {
        var existingOffre= await _context.Offres.FirstOrDefaultAsync(x => x.Id == id);
        if (existingOffre == null)
        {
            return null;
        }

        existingOffre.Titre = offreDto.Titre;
        existingOffre.Description = offreDto.Description;
        existingOffre.Qualifications = offreDto.Qualifications;
        existingOffre.Responsibilities = offreDto.Responsibilities;
        existingOffre.DateDebut = offreDto.DateDebut;
        existingOffre.PostedDate = offreDto.PostedDate;
        existingOffre.RequiredSkills = offreDto.RequiredSkills;
        existingOffre.RequiredLanguages = offreDto.RequiredLanguages;
        existingOffre.Benefits = offreDto.Benefits;
        existingOffre.EstStage = offreDto.EstStage;
        existingOffre.Departement = offreDto.Departement;
        existingOffre.Lieu = offreDto.Lieu;
        existingOffre.ContractType = offreDto.ContractType;
        existingOffre.ContractDuration = offreDto.ContractDuration;
        existingOffre.Gender = offreDto.Gender;
        existingOffre.NiveauEtudesRequis = offreDto.NiveauEtudesRequis;
        existingOffre.EstActive = offreDto.EstActive;

        await _context.SaveChangesAsync();

        return existingOffre;
    }
    public async Task<List<Offre>> GetOffreStageAsync()
    {
        return await _context.Offres.Where(offre => offre.EstStage).ToListAsync();
    }

    public async Task<List<Offre>> GetOffreEmploisAsync()
    {
        return await _context.Offres.Where(offre => !offre.EstStage).ToListAsync();
    }

    public async Task<List<Offre>> GetOffresByDepartementAsync(string departement)
    {
        return await _context.Offres
            .Where(o => o.Departement == departement)
            .ToListAsync();
    }

    public async Task<List<string>> GetDepartementsAsync()
    {
        return await _context.Offres
            .Select(o => o.Departement)
            .Distinct()
            .ToListAsync();
    }
    public async Task<List<string>> GetContractTypesAsync()
    {
        return await _context.Offres
            .Select(o => o.ContractDuration)
            .Distinct()
            .ToListAsync();
    }
    public async Task<List<Offre>> GetOffresByContractTypeAsync(string contracttype)
    {
        return await _context.Offres
            .Where(o => o.ContractDuration == contracttype)
            .ToListAsync();
    }
}
}
