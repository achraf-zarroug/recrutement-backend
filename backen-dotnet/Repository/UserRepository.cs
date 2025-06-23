using backen_dotnet.Data;
using backen_dotnet.Dtos.Account;
using backen_dotnet.Dtos.Offre;
using backen_dotnet.Interfaces;
using backen_dotnet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backen_dotnet.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UserRepository(ApplicationDbContext context,
                                UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<List<AppUser>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<AppUser?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<AppUser?> UpdateAsync(string id, UpdateAppUsereDto appUserDto)
        {
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (existingUser == null)
            {
                return null;
            }

            existingUser.UserName = appUserDto.UserName;
            existingUser.Email = appUserDto.Email;
            existingUser.PhoneNumber = appUserDto.PhoneNumber;
           
            await _context.SaveChangesAsync();

            return existingUser;
        }
        public async Task<AppUser?> DeleteAsync(string id)
        {
            var userModel = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (userModel == null)
            {
                return null;
            }
            _context.Users.Remove(userModel);
            await _context.SaveChangesAsync();
            return userModel;
        }
    }
}
