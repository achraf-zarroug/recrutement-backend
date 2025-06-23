using backen_dotnet.Dtos.Account;
using backen_dotnet.Models;

namespace backen_dotnet.Interfaces
{
    public interface IUserRepository
{
    Task<List<AppUser>> GetAllAsync();

    Task<AppUser?> GetByIdAsync(int id);
    Task<AppUser?> DeleteAsync(string id);
    Task<AppUser?> UpdateAsync(string id, UpdateAppUsereDto appUserDto);
}
}
