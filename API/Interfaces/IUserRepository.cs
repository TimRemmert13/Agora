using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Utilities;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDto> GetUserByUsernameAsync(string username);
        Task<PagedList<UserDto>> GetUsersAsync(UserParams userParams);
        void UpdateUser(AppUser user);
        void DeleteUser(string email);
    }
}