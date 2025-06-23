using backen_dotnet.Models;

namespace backen_dotnet.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);

    }
}
