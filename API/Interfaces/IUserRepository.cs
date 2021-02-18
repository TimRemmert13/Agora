using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserAsync(string email);
        Task<IEnumerable<AppUser>> GetUsersAsync();
        void UpdateUser(AppUser user);

        Task<bool> SaveAllAsync();
        void DeleteUser(string email);

        Task<bool> CreateUserAync(AppUser user);

    }
}