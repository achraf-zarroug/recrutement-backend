using backen_dotnet.Dtos.Offre;
using backen_dotnet.Models;

namespace backen_dotnet.Interfaces
{
    public interface IOffreRepository
{
    Task<List<Offre>> GetAllAsync();
    Task<Offre?> GetByIdAsync(int id);
    Task<Offre?> GetByTitreAsync(string titre);
    Task<Offre> CreateAsync(Offre offreModel);
    Task<Offre?> UpdateAsync(int id,UpdateOffreDto offreDto);
    Task<Offre?> DeleteAsync(int id);
    Task<List<Offre>> GetOffreStageAsync();
    Task<List<Offre>> GetOffreEmploisAsync();
    Task<List<Offre>> GetOffresByDepartementAsync(string departement);
    Task<List<string>> GetDepartementsAsync();
    Task<List<string>> GetContractTypesAsync();
    Task<List<Offre>> GetOffresByContractTypeAsync(string contracttype);

}
}
