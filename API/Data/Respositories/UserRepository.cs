using System;
using System.Collections.Generic;
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

        public async Task<AppUser> CreateUserAsync(AppUser user)
        {
            _context.Users.Add(user);
            bool result = await SaveAllAsync();
            if (result == true)
            {
                return user;
            }
            return null;
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task<AppUser> GetUserByIdAsync(string id)
        {
            Guid parsedId = Guid.Parse(id);
            return await _context.Users.SingleAsync(u => u.Id == parsedId);
        }
        public async Task<bool> DeleteUserAsync(string id)
        {
            Guid parsedId = Guid.Parse(id);
            var user = await _context.Users.SingleAsync(u => u.Id == parsedId);
            _context.Users.Remove(user);
            return await SaveAllAsync();
        }
    }
}