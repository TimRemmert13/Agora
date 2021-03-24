using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Utilities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Respositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateUserAsync(AppUser user)
        {
            await _context.Users.AddAsync(user);
            bool result = await SaveAllAsync();
            return result;
        }

        public async void DeleteUser(string email)
        {
            _context.Users.Remove(await _context.Users.Where(x => x.Email == email).SingleOrDefaultAsync());
        }

        public async Task<UserDto> GetUserAsync(string email)
        {
            return _mapper.Map<AppUser, UserDto>(
                await _context.Users.Include(u => u.ArtWorks).Where(x => x.Email == email).SingleOrDefaultAsync()
                );
        }

        public async Task<PagedList<UserDto>> GetUsersAsync(UserParams userParams)
        {
            var query = _context.Users.AsQueryable();
            var response = await PagedList<UserDto>.CreateAsync(
                query.ProjectTo<UserDto>(_mapper.ConfigurationProvider).AsNoTracking(),
                userParams.PageNumber, userParams.PageSize);
            return response;
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