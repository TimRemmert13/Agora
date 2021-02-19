using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Respositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateUserAync(AppUser user)
        {
            await _context.Users.AddAsync(user);
            bool result = await SaveAllAsync();
            return result;
        }

        public async void DeleteUser(string email)
        {
            _context.Users.Remove(await _context.Users.Where(x => x.Email == email).SingleOrDefaultAsync());
        }

        public async Task<AppUser> GetUserAsync(string email)
        {
            var user = await _context.Users.Where(x => x.Email == email).SingleOrDefaultAsync();
            return user;
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void UpdateUser(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}