using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> CreateUserAsync(AppUser user);
        Task<AppUser> GetUserByIdAsync(string id);
        Task<IEnumerable<AppUser>> GetAllUsersAsync();
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<bool> DeleteUserAsync(string id);
    }
}